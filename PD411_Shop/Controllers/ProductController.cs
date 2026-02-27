using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.Settings;
using PD411_Shop.ViewModels;
using SQLitePCL;

namespace PD411_Shop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public ProductController(CategoryRepository categoryRepository, ProductRepository productRepository, ImageService imageService, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _imageService = imageService;
            _mapper = mapper;
        }


        private async Task<IEnumerable<SelectListItem>> GetSelectCategoriesAsync()
        {
            List<CategoryModel> categories = await _categoryRepository.GetAllAsync();

            IEnumerable<SelectListItem> selectItems = categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()));


            // Приклад того самого коду без використання Select
            //List<SelectListItem> result = new List<SelectListItem>();
            //foreach (var c in categories)
            //{
            //    var item = new SelectListItem(c.Name, c.Id.ToString();
            //    result.Add(item);
            //}

            return selectItems;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository
                .GetAllAsync(new PaginationVM { PageSize = 200 });
            return View(products);
        }


        // GET
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateProductVM
            {
                SelectCategories = await GetSelectCategoriesAsync()
            };

            return View(viewModel);
        }

        // POST
        // [FromForm] - очікує дані у форматі multipart/form-data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateProductVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.SelectCategories = await GetSelectCategoriesAsync();
                return View(vm);
            }

            //ProductModel model = new ProductModel
            //{
            //    Name = vm.Name ?? string.Empty,
            //    Amount = vm.Amount,
            //    Color = vm.Color,
            //    Description = vm.Description,
            //    Price = vm.Price,
            //    CategoryId = vm.CategoryId
            //};

            ProductModel model = _mapper.Map<ProductModel>(vm);

            // Save Image
            if (vm.Image != null)
            {
                model.Image = await _imageService.SaveImageAsync(vm.Image, PathSettings.Products);
            }

            await _productRepository.CreateAsync(model);

            return RedirectToAction("Index");
        }

        // GET
        public async Task<IActionResult> Update(int id)
        {
            var model = await _productRepository.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            //var viewModel = new UpdateProductVM
            //{
            //    Id = model.Id,
            //    Amount = model.Amount,
            //    CategoryId = model.CategoryId,
            //    Color = model.Color,
            //    Description = model.Description,
            //    Price = model.Price,
            //    Name = model.Name,
            //    SelectCategories = await GetSelectCategoriesAsync()
            //};

            var viewModel = _mapper.Map<UpdateProductVM>(model);
            viewModel.SelectCategories = await GetSelectCategoriesAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.SelectCategories = await GetSelectCategoriesAsync();
                return View(viewModel);
            }

            var model = await _productRepository.GetByIdAsync(viewModel.Id);

            if (model != null)
            {
                model.Name = viewModel.Name!;
                model.Description = viewModel.Description;
                model.Price = viewModel.Price;
                model.Amount = viewModel.Amount;
                model.CategoryId = viewModel.CategoryId;
                model.Color = viewModel.Color;

                if (viewModel.Image != null)
                {
                    string? imageName = await _imageService.SaveImageAsync(viewModel.Image, "products");

                    if (imageName != null)
                    {
                        if (!string.IsNullOrEmpty(model.Image))
                        {
                            _imageService.DeleteImage("products", model.Image);
                        }
                    }

                    model.Image = imageName;
                }

                await _productRepository.UpdateAsync(model);
            }

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                if (product.Image != null)
                {
                    _imageService.DeleteImage(PathSettings.Products, product.Image);
                }

                await _productRepository.DeleteAsync(product.Id);
            }
            return RedirectToAction("Index");
        }
    }
}

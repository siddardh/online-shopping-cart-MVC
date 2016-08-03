using OnlineShoppingStore.Domain.Abstract;
using OnlineShoppingStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        private int pageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category,int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null?
                                            repository.Products.Count()
                                            : repository.Products.Where(x =>x.Category == category)
                                            .Count()
                },
                CurrentCategory = category
            };
            return View(model);

            //using mock
            // return View(repository.Products);


            // using manual code to pass between the pages
            //return View(repository.Products
            //    .OrderBy(p=>p.ProductID)
            //    .Skip((page - 1)* pageSize)
            //    .Take(pageSize)
            //    );

        }

    }
}
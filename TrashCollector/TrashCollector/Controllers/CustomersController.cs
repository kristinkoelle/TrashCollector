﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using TrashCollector.Models;
using TrashCollector.ViewModels;


namespace TrashCollector.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult CustomerForm()
        {
            var daysPickup = _context.Days.ToList();
            var viewModel = new CustomerFormViewModel
            {
                Days = daysPickup
            };
            return View(viewModel);
        
        }
        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (customer.Id == 0)
                _context.Customers.Add(customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.FirstName = customer.FirstName;
                customerInDb.LastName = customer.LastName;
                customerInDb.Address = customer.Address;
                customerInDb.DayId = customer.DayId;
                customerInDb.PhoneNumber = customer.PhoneNumber;
                  
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers"); //fix when create roles
        }
        public ViewResult Index ()
        {
            var customers = _context.Customers.Include(c => c.Day).ToList();
            return View(customers);
        }
        public ActionResult Details (int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                Days = _context.Days.ToList()
            };

        
        return View("CustomerForm", viewModel);
        }

    }
}
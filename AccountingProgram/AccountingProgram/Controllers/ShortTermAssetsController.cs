using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AccountingProgram.Controllers
{
    public class ShortTermAssetsController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public ShortTermAssetsController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult STAssetIndex()
        {
            List<Assets> assetList = _context.Assets.ToList();
            return View(assetList);
        }
        public IActionResult IndividualAsset(int id)
        {
            Assets found = _context.Assets.Find(id);
            if(found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult AddAsset()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAsset(Assets asset)
        {
            if(ModelState.IsValid)
            {
                _context.Assets.Add(asset);
                _context.SaveChanges();
              
                Cash c = new Cash();
                c.TransDate = (DateTime)asset.TransDate;
                c.Withdrawl = asset.Cost;
                c.AssetId = asset.AssetId;
                _context.Cash.Add(c);
                _context.SaveChanges();

                return RedirectToAction("STAssetIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult UpdateAsset(int id)
        {
            Assets found = _context.Assets.Find(id);
            return View(found);
        }
        [HttpPost]
        public IActionResult UpdateAsset(Assets updatedAsset)
        {
            Assets old = _context.Assets.Find(updatedAsset.AssetId);
            old.Type = updatedAsset.Type;
            old.Description = updatedAsset.Description;
            old.Cost = updatedAsset.Cost;
            old.TransDate = updatedAsset.TransDate;
            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();

            Cash cash = _context.Cash.First(x => x.AssetId == updatedAsset.AssetId);
            cash.TransDate = (DateTime)updatedAsset.TransDate;
            cash.Withdrawl = updatedAsset.Cost;
            _context.Update(cash);
            _context.SaveChanges();

            return RedirectToAction("STAssetIndex");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class LongTermAssetsController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public LongTermAssetsController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult LongTermAssetIndex()
        {
            List<LongTermAssets> ltassets = _context.LongTermAssets.ToList();
            return View(ltassets);
        }
        public IActionResult IndividualLTAsset(int id)
        {
            LongTermAssets asset = _context.LongTermAssets.Find(id);
            List<AccumulatedDepreciation> adList = _context.AccumulatedDepreciation.Where(x => x.LongTermAssetId == asset.LtassetId).ToList();
            asset.AccumulatedDepreciation = adList;
            return View(asset);
        }
        [HttpGet]
        public IActionResult AddLTAsset()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLTAsset(LongTermAssets asset)
        {
            if (ModelState.IsValid)
            {
                _context.LongTermAssets.Add(asset);
                _context.SaveChanges();
                Cash c = new Cash();
                c.Withdrawl = asset.Amount;
                c.TransDate = asset.PurchaseDate;
                _context.Cash.Add(c);
                _context.SaveChanges();
            }
            return RedirectToAction("LongTermAssetIndex", new { id = asset.LtassetId });
        }
        [HttpGet]
        public IActionResult AddLTAssetandLiability()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLTAssetandLiability(LongTermAssets asset, LongTermLiabilities liability)
        {
            _context.LongTermAssets.Add(asset);
            _context.LongTermLiabilities.Add(liability);
            _context.SaveChanges();

            return RedirectToAction("LongTermAssetIndex", new { id = asset.LtassetId });
        }

    }
}

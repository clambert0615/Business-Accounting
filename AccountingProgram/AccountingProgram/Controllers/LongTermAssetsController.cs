using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        [HttpGet]
        public IActionResult UpdateLTAsset(int id)
        {
            LongTermAssets found = _context.LongTermAssets.Find(id);
            if (found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpPost]
        public IActionResult UpdateLTAsset(LongTermAssets updatedAsset)
        {
            LongTermAssets oldAsset = _context.LongTermAssets.Find(updatedAsset.LtassetId);
            if(ModelState.IsValid)
            {
                oldAsset.Item = updatedAsset.Item;
                oldAsset.Description = updatedAsset.Description;
                oldAsset.Amount = updatedAsset.Amount;
                oldAsset.Balance = updatedAsset.Balance;
                oldAsset.PurchaseDate = updatedAsset.PurchaseDate;
                oldAsset.LifeRemaining = updatedAsset.LifeRemaining;
                oldAsset.UsefulLife = updatedAsset.UsefulLife;
                _context.Entry(oldAsset).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(oldAsset);
                _context.SaveChanges();

                return RedirectToAction("LongTermAssetIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult SellAsset(int id)
        {
            LongTermAssets found = _context.LongTermAssets.Find(id);
            if(found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("LongTermAssetIndex");

            }
        }
        [HttpPost]
        public IActionResult SellAsset(int ltassetId, DateTime date, decimal amount )
        {
            LongTermAssets asset = _context.LongTermAssets.Find(ltassetId);
            asset.DisposalDate = date;
            if (amount != 0)
            {
                if (asset.Balance > amount)
                {
                    asset.Loss = asset.Balance - amount;
                }
                else
                {
                    asset.Gain = amount - asset.Balance;
                }
            }
            asset.Amount -= asset.Amount;

            _context.Update(asset);
            _context.SaveChanges();

            List<AccumulatedDepreciation> adList = _context.AccumulatedDepreciation.Where(x => x.LongTermAssetId == ltassetId).ToList();
            foreach (AccumulatedDepreciation ad in adList)
            {
                ad.Amount -= ad.Amount;
                _context.Update(ad);
                _context.SaveChanges();
            }

            if (amount != 0)
            {
                Cash cash = new Cash
                {
                    TransDate = date,
                    Deposit = amount
                };
                _context.Cash.Add(cash);
                _context.SaveChanges();
            }

            return RedirectToAction("LongTermAssetIndex");
        }
    }
}

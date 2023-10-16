using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using System.Data.SqlClient;
using System.Reflection;

namespace Projekt.Controllers
{
    public class HotellbokningController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inloggning()
        {
            return View();
        }

        //[HttpPost] 
        //public IActionResult Inloggning(string epost, string losenord)
        //{
        //    GuestMetoder guestMetoder = new GuestMetoder();
        //    bool guestExists = guestMetoder.GuestExist(epost, losenord);

        //    if (guestExists)
        //    {
        //        return RedirectToAction("SelectHotellWithDataSet");

        //    }
        //    else
        //    {
        //        ViewBag.ErrorMessage = "Felaktiga inloggningsuppgifter.";
        //        return View();
        //    }
        //}



        [HttpPost]
        public IActionResult Inloggning(string epost, string losenord)
        {
            GuestMetoder guestMetoder = new GuestMetoder();
            int guestID = guestMetoder.GetGuestId(epost, losenord); // Här antas att du har en metod som hämtar användarens/gästens ID baserat på e-post och lösenord.

            if (guestID > 0) // Om användaren/gästen hittades i databasen
            {
                // Spara användarens/gästens ID i sessionen
                HttpContext.Session.SetInt32("GuestID", guestID);

                return RedirectToAction("SelectHotellWithDataSet");
            }
            else
            {
                ViewBag.ErrorMessage = "Felaktiga inloggningsuppgifter.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult NewGuest()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult NewGuest(Guest g)
        //{
        //    GuestMetoder gm = new GuestMetoder();
        //    int i = 0;
        //    string error = "";
        //    i = gm.InsertGuest(g, out error);
        //    ViewBag.error = error;
        //    ViewBag.antal = i;
        //    if (i == 1) { return RedirectToAction("SelectHotellWithDataSet"); }
        //    else { return View(); }
        //}



        [HttpPost]
        public IActionResult NewGuest(Guest g)
        {
            GuestMetoder gm = new GuestMetoder();
            int guestID = 0; // Initiera användar-ID
            string error = "";
            guestID = gm.InsertGuest(g, out error);

            if (guestID > 0) // Om användaren/gästen registrerades framgångsrikt och har ett giltigt ID
            {
                // Spara användar-ID i sessionen
                HttpContext.Session.SetInt32("GuestID", guestID);

                return RedirectToAction("SelectHotellWithDataSet");
            }
            else
            {
                ViewBag.error = error;
                ViewBag.antal = 0; // Eller något annat värde beroende på ditt behov
                return View();
            }
        }



        public IActionResult SelectHotellWithDataSet()
        {
            List<Hotell> HotellList = new List<Hotell>();
            HotellMetoder hm = new HotellMetoder();
            string error = "";
            HotellList = hm.GetHotellWithDataSet(out error);

            int guestID = HttpContext.Session.GetInt32("GuestID") ?? 0; // Om inte guestID finns i sessionen, används 0 som standardvärde.
            ViewBag.GuestID = guestID;

            ViewBag.error = error;
            return View(HotellList);

        }

        public IActionResult Details(int id)
        {
            Hotell hotell = new Hotell();
            HotellMetoder hm = new HotellMetoder();
            hotell = hm.GetHotell(id, out string error);
            HttpContext.Session.SetInt32("HotellID", id);
            ViewBag.HotellID = id;

            ViewBag.error = error;
            return View(hotell);
        }

        [HttpGet]
        public IActionResult Bokning()
        {
            RumstypMetoder rm = new RumstypMetoder();
            ViewModelBR myModel = new ViewModelBR
            {
                RumstypLista = rm.GetRumstypLista(out string errormsg)

            };
            ViewBag.error = errormsg;

            return View(myModel);
        }


        //test från och med här

        //[HttpPost]
        //public ActionResult Bokning(ViewModelBR model)
        //{
        //    BokningMetoder bm = new BokningMetoder();
        //    int i = 0;
        //    string error = "";
        //    i = bm.InsertBokning(model, out error);

        //    if (i == 1) { return RedirectToAction("Bokningsdetaljer"); }
        //    else { return View("Bokning"); }
        //}

        //[HttpPost]
        //public ActionResult Bokning(ViewModelBR br)
        //{
        //    BokningMetoder bm = new BokningMetoder();
        //    int i = 0;
        //    string error = "";

        //    //Hämtar hotell-id från sessionen
        //    int hotellID = HttpContext.Session.GetInt32("HotellID") ?? 0; // Använd ?? 0 för att ange ett standardvärde om ID inte finns i sessionen

        //    //Hämtar rumstyp-id från rumstyp-namnet som valdes i drop-downlistan.
        //    RumstypMetoder rtm = new RumstypMetoder();
        //    int rumstypID = rtm.GetRumstypID(br);

        //    //Hämtar rum-id baserat på tidigare hotell-id och rumstyp-id. 
        //    RumMetoder rm = new RumMetoder();
        //    int rumID = rm.GetRumID(hotellID, rumstypID);

        //    //Hämtar gäst-id från sessionen
        //    int guestID = HttpContext.Session.GetInt32("GuestID") ?? 0;

        //    int pris = rtm.GetPris(rumstypID);

        //    TimeSpan vistelse = br.CheckUtDatum - br.CheckInDatum;
        //    int antalDagar = vistelse.Days;

        //    int totalKostnad = antalDagar * pris;


        //    Bokning bokningAttSpara = new Bokning
        //    {
        //        CheckInDatum = br.CheckInDatum,
        //        CheckUtDatum = br.CheckUtDatum,
        //        GuestId = guestID,
        //        RumId = rumID,
        //        TotalKostnad = totalKostnad
        //    };

        //    i = bm.InsertBokning(bokningAttSpara, out error);
        //    ViewBag.error = error;
        //    if (i == 1)
        //    {
        //        int bokningID = bm.GetBokningID(bokningAttSpara);
        //        return RedirectToAction("Bokningsdetaljer", new { bokningID });
        //    }
        //    else { return View("Bokning"); }

        //}

        [HttpPost]
        public ActionResult Bokning(ViewModelBR br)
        {
            BokningMetoder bm = new BokningMetoder();
            int i = 0;
            string error = "";

            //Hämtar hotell-id från sessionen
            int hotellID = HttpContext.Session.GetInt32("HotellID") ?? 0; // Använd ?? 0 för att ange ett standardvärde om ID inte finns i sessionen

            //Hämtar rumstyp-id från rumstyp-namnet som valdes i drop-downlistan.
            RumstypMetoder rtm = new RumstypMetoder();
            int rumstypID = rtm.GetRumstypID(br);

            //Hämtar rum-id baserat på tidigare hotell-id och rumstyp-id. 
            RumMetoder rm = new RumMetoder();
            int rumID = rm.GetRumID(hotellID, rumstypID);

            //Hämtar gäst-id från sessionen
            int guestID = HttpContext.Session.GetInt32("GuestID") ?? 0;

            int pris = rtm.GetPris(rumstypID);

            TimeSpan vistelse = br.CheckUtDatum - br.CheckInDatum;
            int antalDagar = vistelse.Days;

            int totalKostnad = antalDagar * pris;


            Bokning bokningAttSpara = new Bokning
            {
                CheckInDatum = br.CheckInDatum,
                CheckUtDatum = br.CheckUtDatum,
                GuestId = guestID,
                RumId = rumID,
                TotalKostnad = totalKostnad
            };

            i = bm.InsertBokning(bokningAttSpara, out error);
            ViewBag.error = error;

            if (i == 1)
            {
                int bokningID = bm.GetBokningID(bokningAttSpara);
                return RedirectToAction("Bokningsdetaljer", new { bokningID });
            }
            else
            {
                ViewModelBR myModel = new ViewModelBR
                {
                    RumstypLista = rtm.GetRumstypLista(out string errormsg)

                };
                ViewBag.error2 = errormsg;

                return View("Bokning", myModel);

            }

        }

    

        public ActionResult Bokningsdetaljer(int bokningID)
        {
            BokningMetoder bm = new BokningMetoder();
            Bokning bokning = bm.GetBokning(bokningID); // Antag att du har en GetBokning-metod som hämtar bokningsinformationen.
            return View(bokning);
        }


        //public IActionResult MinProfil()
        //{
        //    List<BokningHotell> BokningHotellList = new List<BokningHotell>();
        //    BokningMetoder hm = new BokningMetoder();
        //    string error = "";

        //    int guestID = HttpContext.Session.GetInt32("GuestID") ?? 0;

        //    BokningHotellList = hm.GetBokninglWithDataSet(guestID, out error);

        //    ViewBag.error = error;
        //    return View(BokningHotellList);
        //}


        [HttpGet]
        public IActionResult MinProfil()
        {
            BokningMetoder bm = new BokningMetoder();
            int guestID = HttpContext.Session.GetInt32("GuestID") ?? 0;


            ProfilBokningarVM myModel = new ProfilBokningarVM
            {
                BokningHotellLista = bm.GetBokningHotell(guestID, out string errormsg),
                GuestLista = bm.GetGuest(guestID, out string errormsg2)

            };

            
            ViewBag.error = "1: " + errormsg + "2:" + errormsg2;

            return View(myModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            GuestMetoder gm = new GuestMetoder();
            var Guest = gm.GetGuest(Id, out string error);
            ViewBag.error = error;
            return View(Guest);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfirm(Guest g)
        {
            GuestMetoder gm = new GuestMetoder();
            string error = "";
            int i = gm.EditGuest(g, out error);
            return RedirectToAction("MinProfil");
        }


    }
    
}

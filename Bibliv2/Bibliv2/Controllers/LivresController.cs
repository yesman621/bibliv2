using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Bibliv2.Models;
using System.Diagnostics;

namespace Bibliv2.Controllers
{
    [RoutePrefix("api/livres")]
    public class LivresController : ApiController
    {
        private BIBLIEntities db = new BIBLIEntities();

        //recherche ouvrage(s) par auteur
        [HttpGet]
        [Route("~api/auteur/{authorId}/livres")]
        public IQueryable<Livre> GetLivresparAuteur(string auteur)
        {
            
            Debug.WriteLine("/ test");
            IQueryable<Livre> entryPoint = (from ep in db.Livres
                                            join e in db.Auteurs on ep.idAut equals e.idAut
                                            where e.nomAuteur == auteur
                                            select new Livre
                                            {
                                                idliv = ep.idliv,
                                                titre = ep.titre,
                                                anneeCrea = ep.anneeCrea,
                                                descr = ep.descr,
                                                idAut = ep.idAut

                                            });
            if (entryPoint == null)
                Debug.WriteLine("/rien dt");

            foreach (Livre r in entryPoint)
                Debug.WriteLine(r.titre + "/aut");

            return entryPoint;
        }
        //recherche ouvrage par maison edition
        [HttpGet]
        [Route("~api/maisonedition/{meId}/livres")]
        public IQueryable<Livre> GetLivresparMaisonEdition(string mEd)
        {
           
            Debug.WriteLine("/ test");
            IQueryable<Livre> entryPoint = (from e in db.Livres
                                           join ep in db.Exemplaires on e.idliv equals ep.idliv
                                            where ep.idME == mEd
                                            select new Livre
                                            {
                                                idliv = e.idliv,
                                                titre = e.titre,
                                                anneeCrea = e.anneeCrea,
                                                descr = e.descr,
                                                idAut = e.idAut

                                            }
                                            );



            if (entryPoint == null)
                Debug.WriteLine("/rien dt");

            foreach (Livre r in entryPoint)
                Debug.WriteLine(r.titre + "/aut");

            return entryPoint;
        }

        //recherche ouvrage par titre
        [HttpGet]
        [Route("~api/livres/{titre}/livres")]
        public IQueryable<Livre> LivresparTitre(string titre)
        {
            Debug.WriteLine("/ test");
            IQueryable<Livre> entryPoint = (from ep in db.Livres
                                            
                                            where ep.titre == titre
                                            select new Livre
                                            {
                                                idliv = ep.idliv,
                                                titre = ep.titre,
                                                anneeCrea = ep.anneeCrea,
                                                descr = ep.descr,
                                                idAut = ep.idAut

                                            });
            if (entryPoint == null)
                Debug.WriteLine("/rien dt");

            foreach (Livre r in entryPoint)
                Debug.WriteLine(r.titre + "/aut");

            return entryPoint;
        }

        private bool MaisonEditionExists(string id)
        {
            return db.MaisonEditions.Count(e => e.idME == id) > 0;
        }
        //Ajout nouvelle maison edition
        // POST: api/MaisonEditions
        [HttpPost]
        [ResponseType(typeof(MaisonEdition))]
        public IHttpActionResult PostMaisonEdition(MaisonEdition maisonEdition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MaisonEditions.Add(maisonEdition);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MaisonEditionExists(maisonEdition.idME))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = maisonEdition.idME }, maisonEdition);
        }

        private bool AuteurExists(string id)
        {
            return db.Auteurs.Count(e => e.idAut == id) > 0;
        }

        //Ajout auteur
        // POST: api/Auteurs
        [HttpPost]
        [ResponseType(typeof(Auteur))]
        public IHttpActionResult PostAuteur(Auteur auteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Auteurs.Add(auteur);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AuteurExists(auteur.idAut))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = auteur.idAut }, auteur);
        }
        
       //juste voir si le lien bd marche
        // GET: api/Livres
                [HttpGet]
        public IQueryable<Livre> GetLivres()
        {
            DbSet<Livre> d = db.Livres;
            
            foreach(Livre a in d)
            Debug.WriteLine(a.titre+"/");
            
            return d;
        }
        /*
               // GET: api/Livres/5
               [Route("{id:int}")]
               [ResponseType(typeof(Livre))]
               public IHttpActionResult GetLivre(string id)
               {
                   Livre livre = db.Livres.Find(id);
                   if (livre == null)
                   {
                       return NotFound();
                   }
                   Debug.WriteLine(livre.titre + "/l");
                   return Ok(livre);
               }*/

        //modification livre existant et gestion acces concurrent
        // PUT: api/Livres/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLivre(string id, byte[] rowVersion)
        {
            if (id == null)
            {
                return  StatusCode(HttpStatusCode.BadRequest);
            }

            var LivreToUpdate = db.Livres.Find(id);
            

            if (LivreToUpdate == null)
            {
                return NotFound();
            }
            db.Entry(LivreToUpdate).State = EntityState.Modified;
            try
            {
                db.Entry(LivreToUpdate).OriginalValues["RowVersion"] = rowVersion;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Livre)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty,
                        "Unable to save changes. The book was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Livre)databaseEntry.ToObject();
                    ModelState.AddModelError(string.Empty,
                     "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        );
                    LivreToUpdate.RowVersion = databaseValues.RowVersion;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool ExemplaireExists(string id)
        {
            return db.Exemplaires.Count(e => e.idEx == id) > 0;
        }

        //ajout nouvel exemplaire
        // POST: api/Exemplaires
        [HttpPost]
        [ResponseType(typeof(Exemplaire))]
        public IHttpActionResult PostExemplaire(Exemplaire exemplaire)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exemplaires.Add(exemplaire);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ExemplaireExists(exemplaire.idEx))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = exemplaire.idEx }, exemplaire);
        }


        //ajout nouveau livre
        // POST: api/Livres
        [HttpPost]
        [ResponseType(typeof(Livre))]
        public IHttpActionResult PostLivre(Livre livre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Livres.Add(livre);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LivreExists(livre.idliv))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = livre.idliv }, livre);
        }
        //enlever emprunt d'un lecteur
        // DELETE: api/Livres/5
        [ResponseType(typeof(Livre))]
        public IHttpActionResult DeleteEmprunt(string idEmprunteur,string idExemplaire)
        {
           /*
            Emprunt emprunt =  (from ep in db.Emprunts
                                
                                            join e in db.Emprunteurs on ep.numCarte equals e.numCarte
                                            join ex in db.Exemplaires on ep.idEx equals ex.idEx
                                            where e.numCarte == idEmprunteur
                                            where ex.idEx== idExemplaire
                                            select new Emprunt
                      {
                          
                      }).First();
            */

            Emprunt emprunt = db.Emprunts.Where(a => a.numCarte == idEmprunteur).Where(a => a.idEx == idExemplaire).Single();      
            if (emprunt == null)
            {
                return NotFound();
            }

            db.Emprunts.Remove(emprunt);
            db.SaveChanges();

            return Ok(emprunt);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        private bool LivreExists(string id)
        {
            return db.Livres.Count(e => e.idliv == id) > 0;
        }
    }
}
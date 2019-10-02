using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class TourController : ApiController
    {
        // GET api/tour
        public IHttpActionResult GetAllTours()
        {
            IList<TourViewModel> tours = null;

            using (var ctx = new ExploreCaliforniaEntities())
            {
                tours = ctx.Tours.Include("Explore")
                    .Select(t => new TourViewModel()
                    {
                        TourId = t.TourId,
                        Description = t.Description,
                        Name = t.Name,
                        Notes = t.Notes,
                        Price = t.Price
                    }).ToList<TourViewModel>();
            }

            if (tours.Count == 0)
            {
                return NotFound();
            }

            return Ok(tours);
        }

        // GET api/tour/5
        public IHttpActionResult GetTourByID(int id)
        {
            IList<TourViewModel> tours = null;

            using (var ctx = new ExploreCaliforniaEntities())
            {
                tours = ctx.Tours
                    .Where(t => t.TourId == id)
                    .Select(t => new TourViewModel()
                    {
                        TourId = t.TourId,
                        Description = t.Description,
                        Name = t.Name,
                        Notes = t.Notes,
                        Price = t.Price
                    }).ToList<TourViewModel>();
            }

            if (tours.Count == 0)
            {
                return NotFound();
            }

            return Ok(tours);
        }

        // POST api/<controller>
        public IHttpActionResult PostNewTour(TourViewModel tour)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new ExploreCaliforniaEntities())
            {
                ctx.Tours.Add(new Tour()
                {
                    TourId = tour.TourId,
                    Description = tour.Description,
                    Name = tour.Name,
                    Notes = tour.Notes,
                    Price = tour.Price
                });

                ctx.SaveChanges();
            }

            return Ok();
        }
        // PUT api/<controller>/5
        public IHttpActionResult Put(TourViewModel tour)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx =  new ExploreCaliforniaEntities())
            {
                var existingTour = ctx.Tours.FirstOrDefault(t => t.TourId == tour.TourId);

                if (existingTour != null)
                {
                    existingTour.Name = tour.Name;
                    existingTour.Description = tour.Description;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid tour id");

            using (var ctx = new ExploreCaliforniaEntities())
            {
                var tour = ctx.Tours
                    .FirstOrDefault(t => t.TourId == id);

                if (tour != null)
                {
                    ctx.Entry(tour).State = EntityState.Deleted;
                    ctx.SaveChanges();
                }
                else
                {
                    return BadRequest(String.Format("The id: {0} provided is not there anymore", id));
                }
            }

            return Ok();
        }
    }
}
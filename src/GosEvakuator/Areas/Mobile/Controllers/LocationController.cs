using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GosEvakuator.Data;
using Microsoft.AspNetCore.Authorization;
using GosEvakuator.Areas.Mobile.Models;

namespace GosEvakuator.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class LocationController : Controller
    {
        public enum ErrorCode
        {
            LocationNameAndNotesRequired,
            LocationIDInUse,
            RecordNotFound,
            CouldNotCreateItem,
            CouldNotUpdateItem,
            CouldNotDeleteItem
        }

        private readonly ApplicationDbContext dbContext;

        public LocationController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            // return Ok(dbContext.Locations);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendList([FromBody]LocationsModel locationsModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // dbContext.Locations.AddRange(locationsModel.Items);
                    await dbContext.SaveChangesAsync();

                    return Ok(locationsModel);
                }
                catch (Exception e)
                {
                    var t = e;
                    return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
                }
            }

            return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody]LocationModel locationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Add(locationModel);
                    await dbContext.SaveChangesAsync();

                    return Ok(locationModel);
                }
                catch (Exception e)
                {
                    var t = e;
                    return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
                }
            }

            return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
        }

        [HttpPut]
        public IActionResult Edit([FromBody] LocationModel item)
        {
            try
            {
               /*  var existingItem = dbContext.Locations.Find(item.ID);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }

                dbContext.Locations.Update(item);*/
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }

            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] LocationModel item)
        {
            try
            {
                /*var existingItem = dbContext.Locations.Find(item.ID);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }

                dbContext.Locations.Remove(item);*/
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }

            return NoContent();
        }
    }
}
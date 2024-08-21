using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BurgerMania.Data;
using BurgerMania.Models;
using Microsoft.EntityFrameworkCore;
using BurgerMania.DTOs;
using BurgerMania.Mappers;

namespace BurgerMania.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BurgerController : ControllerBase
    {
        private readonly BurgerManiaContext _context;
        private readonly BurgerMapper _burgerMapper;

        public BurgerController(BurgerManiaContext context)
        {
            _context = context;
            _burgerMapper = new BurgerMapper();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Burger>>> GetAllBurgers()
        //{
        //    return await _context.Burgers.ToListAsync();
        //}

        //[HttpGet("{id}")]

        //public async Task<ActionResult<Burger>> GetBurgerByID(Guid id)
        //{
        //    var burger = await _context.Burgers.FindAsync(id);

        //    if (burger == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(burger);
        //}

        //[HttpGet("/burgerid")]
        //public async Task<ActionResult<Guid>> GetBurgerID(string name, string category)
        //{
        //    var existingBurger = await _context.Burgers.FirstOrDefaultAsync(b => b.Name == name && b.Category == category);
        //    if (existingBurger == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(existingBurger.burgerId);
        //    }
        //}

        //[HttpPost]

        //public async Task<ActionResult<Burger>> AddBurger([FromBody] Burger burger)
        //{
        //    burger.burgerId = Guid.NewGuid();

        //    var existingBurger = await _context.Burgers.FirstOrDefaultAsync(b => b.Name == burger.Name && b.Category == burger.Category);

        //    if (existingBurger != null)
        //    {
        //        return Conflict($"Burger with Name {burger.Name} and category {burger.Category} already exists!");
        //    }

        //    _context.Burgers.Add(burger);

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }

        //    catch(DbUpdateException)
        //    {
        //        return StatusCode(500, "Error occured when saving burger to DB");
        //    }

        //    return CreatedAtAction(nameof(GetBurgerByID), new {id = burger.burgerId}, burger);

        //}


        //[HttpPut("{id}")]

        //public async Task<IActionResult> UpdateBurger(Guid id, Burger burger)
        //{
        //    if(id != burger.burgerId)
        //    {
        //        return BadRequest();
        //    }

        //    //burger = new Burger 
        //    //{ burgerId = id, Name = burger.Name,
        //    //Category = burger.Category, Price = burger.Price};

        //    _context.Entry(burger).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BurgerExists(id))
        //        {
        //            return NotFound($"Said burger with burger id {id} doesn't exist");
        //        }
        //        else
        //        {
        //            return Conflict("Burger was modified by another user. Pls try again");
        //        }
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Something went wrong when saving burger update");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Unexpected Error occured");
        //    }

        //    return NoContent();

        //}


        //[HttpDelete]
        //public async Task<IActionResult> RemoveBurger(Guid id)
        //{
        //    var burgerTBDeleted = await _context.Burgers.FindAsync(id);

        //    if (burgerTBDeleted == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Burgers.Remove(burgerTBDeleted);

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //            return Conflict("Burger was modified by another user. Pls try again");

        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Something went wrong when deleting burger");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Unexpected Error occured");
        //    }

        //    return NoContent();

        //}

        //private bool BurgerExists(Guid id)
        //{
        //    return _context.Burgers.Any(b => b.burgerId == id);
        //}


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BurgerDTO>>> GetAllBurgers()
        {
            var burgers = await _context.Burgers.ToListAsync();

            var burgerDTOs =
                burgers.Select(burger => _burgerMapper.MapToDTO(burger)).ToList();

            return Ok(burgerDTOs);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<BurgerDTO>> GetBurgerByID(Guid id)
        {
            var burger = await _context.Burgers.FindAsync(id);

            if (burger == null)
            {
                return NotFound();
            }

            var burgerdto = _burgerMapper.MapToDTO(burger);

            return Ok(burgerdto);
        }

        [HttpGet("/burgerid")]
        public async Task<ActionResult<Guid>> GetBurgerID(string name, string category)
        {
            var existingBurger = await _context.Burgers.FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower() && b.Category.ToLower() == category.ToLower());
            if (existingBurger == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(existingBurger.burgerId);
            }
        }

        [HttpPost]

        public async Task<ActionResult<BurgerDTO>> AddBurger([FromBody] BurgerDTO burgerdto)
        {
            burgerdto.burgerId = Guid.NewGuid();

            var existingBurger = 
                await _context.Burgers.FirstOrDefaultAsync
                (b => b.Name == burgerdto.Name && b.Category == burgerdto.Category);

            if (existingBurger != null)
            {
                return Conflict($"Burger with Name {burgerdto.Name} and category {burgerdto.Category} already exists!");
            }

            var burger = _burgerMapper.MapToEntity(burgerdto);

            _context.Burgers.Add(burger);

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateException)
            {
                return StatusCode(500, "Error occured when saving burger to DB");
            }

            return CreatedAtAction(nameof(GetBurgerByID), new { id = burger.burgerId }, burgerdto);

        }


        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateBurger(Guid id, BurgerDTO burgerdto)
        {
            if (id != burgerdto.burgerId)
            {
                return BadRequest();
            }

            //burger = new Burger 
            //{ burgerId = id, Name = burger.Name,
            //Category = burger.Category, Price = burger.Price};

            var burger = await _context.Burgers.FirstOrDefaultAsync(b => b.burgerId == id);

            if (burger == null)
            {
                return NotFound();
            }

            //var burgerToMap = _burgerMapper.MapToEntity(burgerdto);

            if(!string.IsNullOrEmpty(burgerdto.Name))
            {
                burger.Name = burgerdto.Name;
            }

            if (!string.IsNullOrEmpty(burgerdto.Category))
            {
                burger.Category = burgerdto.Category;
            }

            if(burgerdto.Price.HasValue)
            {
                burger.Price = burgerdto.Price;
            }
            

            _context.Entry(burger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BurgerExists(id))
                {
                    return NotFound($"Said burger with burger id {id} doesn't exist");
                }
                else
                {
                    return Conflict("Burger was modified by another user. Pls try again");
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Something went wrong when saving burger update");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unexpected Error occured");
            }

            return NoContent();

        }


        [HttpDelete]
        public async Task<IActionResult> RemoveBurger(Guid id)
        {
            var burgerTBDeleted = await _context.Burgers.FindAsync(id);

            if (burgerTBDeleted == null)
            {
                return NotFound();
            }

            _context.Burgers.Remove(burgerTBDeleted);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Burger was modified by another user. Pls try again");

            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Something went wrong when deleting burger");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected Error occured");
            }

            return NoContent();

        }

        private bool BurgerExists(Guid id)
        {
            return _context.Burgers.Any(b => b.burgerId == id);
        }
    }
}

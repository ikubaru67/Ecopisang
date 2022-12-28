using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pisang.Models;
using pisang.Data;

namespace pisang.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PisangItemsController : ControllerBase
{
    private readonly UserContext dbContext;

    public PisangItemsController(UserContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // GET: api/PisangItems
    [HttpGet]
    public async Task<ActionResult> GetPisangItems()
    {
        return Ok(await dbContext.pisangItemDTOs.ToListAsync());
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<PisangItemDTO>> GetPisangItem(int id)
    {
        var pisangItem = await dbContext.pisangItemDTOs.FindAsync(id);

        if (pisangItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(pisangItem);
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPisangItem(int id, PisangItemDTO pisangDTO)
    {
        if (id != pisangDTO.Id)
        {
            return BadRequest();
        }

        var pisangItem = await dbContext.pisangItemDTOs.FindAsync(id);
        if (pisangItem == null)
        {
            return NotFound();
        }
        
        pisangItem.Jenis = pisangDTO.Jenis;
        pisangItem.Harga = pisangDTO.Harga;
        pisangItem.Stok = pisangDTO.Stok;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!PisangItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult> AddPisang(AddPisang addPisang)
    {
        var pisangItem = new PisangItemDTO()
        {
            Jenis = addPisang.Jenis,
            Harga = addPisang.Harga,
            Stok = addPisang.Stok
        };
        
        await dbContext.pisangItemDTOs.AddAsync(pisangItem);
        await dbContext.SaveChangesAsync();

        return Ok(pisangItem);
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePisangItem(int id)
    {
        var pisangItem = await dbContext.pisangItemDTOs.FindAsync(id);
        if (pisangItem == null)
        {
            return NotFound();
        }

        dbContext.pisangItemDTOs.Remove(pisangItem);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool PisangItemExists(int id)
    {
        return dbContext.pisangItemDTOs.Any(e => e.Id == id);
    }

    private static PisangItemDTO ItemToDTO(PisangItemDTO pisangItem) =>
       new PisangItemDTO
       {
           Id = pisangItem.Id,
           Jenis = pisangItem.Jenis,
           Harga = pisangItem.Harga,
           Stok = pisangItem.Stok
       };
}
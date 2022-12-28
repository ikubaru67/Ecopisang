using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pisang.Models;
using pisang.Data;

namespace pisang.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PenjualItemsController : ControllerBase
{
    private readonly UserContext dbContext;

    public PenjualItemsController(UserContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // GET: api/PisangItems
    [HttpGet]
    public async Task<ActionResult> GetPenjualItems()
    {
        return Ok(await dbContext.penjualItemDTOs.ToListAsync());
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<PenjualItemDTO>> GetPenjualItem(int id)
    {
        var penjualItem = await dbContext.penjualItemDTOs.FindAsync(id);

        if (penjualItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(penjualItem);
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPenjualItem(int id, PenjualItemDTO penjualDTO)
    {
        if (id != penjualDTO.Id)
        {
            return BadRequest();
        }

        var penjualItem = await dbContext.penjualItemDTOs.FindAsync(id);
        if (penjualItem == null)
        {
            return NotFound();
        }

        penjualItem.Nama = penjualDTO.Nama;
        penjualItem.Email = penjualDTO.Email;
        penjualItem.Password = penjualDTO.Password;
        penjualItem.Nomor = penjualDTO.Nomor;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!PenjualItemExists(id))
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
    public async Task<ActionResult> AddPenjual(AddPenjual addPenjual)
    {
        var penjualItem = new PenjualItemDTO()
        {
            Nama = addPenjual.Nama,
            Email = addPenjual.Email,
            Password = addPenjual.Password,
            Nomor = addPenjual.Nomor
        };
        
        await dbContext.penjualItemDTOs.AddAsync(penjualItem);
        await dbContext.SaveChangesAsync();

        return Ok(penjualItem);
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePenjualItem(int id)
    {
        var penjualItem = await dbContext.penjualItemDTOs.FindAsync(id);
        if (penjualItem == null)
        {
            return NotFound();
        }

        dbContext.penjualItemDTOs.Remove(penjualItem);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool PenjualItemExists(int id)
    {
        return dbContext.penjualItemDTOs.Any(e => e.Id == id);
    }

    private static PenjualItemDTO ItemToDTO(PenjualItemDTO penjualItem) =>
       new PenjualItemDTO
       {
           Id = penjualItem.Id,
           Nama = penjualItem.Nama,
           Email = penjualItem.Email,
           Password = penjualItem.Password,
           Nomor = penjualItem.Nomor
       };
}
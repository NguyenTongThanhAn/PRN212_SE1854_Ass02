using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;


namespace NewsManagementSystem.DAL.Repositories.Tag;

public class TagRepo : ITagRepo
{
    private readonly FUNewsManagementContext _context;

    public TagRepo(FUNewsManagementContext context)
    {
        _context = context;
    }

    public async Task CreateTagAsync(BusinessObject.Entities.Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(int tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null)
            return;

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BusinessObject.Entities.Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task<List<BusinessObject.Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds)
    {
        return await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
    }

    public async Task UpdateTagAsync(BusinessObject.Entities.Tag tag)
    {
        var existingTag = await _context.Tags.FindAsync(tag.TagId);
        if (existingTag == null)
            return;

        existingTag.TagName = tag.TagName;
        existingTag.Note = tag.Note;
        // Update other properties if needed

        await _context.SaveChangesAsync();
    }

    
}
using NewsManagementSystem.DAL.Repositories.Tag;

namespace NewsManagementSystem.BLL.Services.Tag;


public class TagService : ITagService
{
    private readonly ITagRepo _tagRepo;

    public TagService(ITagRepo tagRepo)
    {
        _tagRepo = tagRepo;
    }

    public Task CreateTagAsync(BusinessObject.Entities.Tag tag) => _tagRepo.CreateTagAsync(tag);

    public Task DeleteTagAsync(int tagId) => _tagRepo.DeleteTagAsync(tagId);

    public Task<List<BusinessObject.Entities.Tag>> GetAllTagsAsync() => _tagRepo.GetAllTagsAsync();

    public Task<List<BusinessObject.Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds) => _tagRepo.GetTagsByIdsAsync(tagIds);

    

    public Task UpdateTagAsync(BusinessObject.Entities.Tag tag) => _tagRepo.UpdateTagAsync(tag);

    
}
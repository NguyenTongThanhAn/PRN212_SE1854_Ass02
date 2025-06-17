namespace NewsManagementSystem.BLL.Services.Tag;

public interface ITagService
{
    Task<List<BusinessObject.Entities.Tag>> GetAllTagsAsync();
    Task<List<BusinessObject.Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds);
    Task CreateTagAsync(BusinessObject.Entities.Tag tag);
    Task UpdateTagAsync(BusinessObject.Entities.Tag tag);
    Task DeleteTagAsync(int tagId);
}
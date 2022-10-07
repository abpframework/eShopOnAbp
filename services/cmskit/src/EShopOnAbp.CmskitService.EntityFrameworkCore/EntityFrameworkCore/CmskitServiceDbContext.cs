using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

[ConnectionStringName(CmskitServiceDbProperties.ConnectionStringName)]
public class CmskitServiceDbContext : AbpDbContext<CmskitServiceDbContext>, ICmskitServiceDbContext, ICmsKitDbContext
{
    public CmskitServiceDbContext(DbContextOptions<CmskitServiceDbContext> options)
        : base(options)
    {

    }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<CmsUser> User { get; set; }

    public DbSet<UserReaction> Reactions { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<EntityTag> EntityTags { get; set; }

    public DbSet<Page> Pages { get; set; }

    public DbSet<Blog> Blogs { get; set; }

    public DbSet<BlogPost> BlogPosts { get; set; }

    public DbSet<BlogFeature> BlogFeatures { get; set; }

    public DbSet<MediaDescriptor> MediaDescriptors { get; set; }

    public DbSet<MenuItem> MenuItems { get; set; }

    public DbSet<GlobalResource> GlobalResources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        FeatureConfigurer.Configure();

        modelBuilder.ConfigureCmskitService();
        modelBuilder.ConfigureCmsKit();
    }
}

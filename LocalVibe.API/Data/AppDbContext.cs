using LocalVibe.API.Entities;
using LocalVibe.API.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace LocalVibe.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Post ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Title)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(p => p.Description)
                  .HasMaxLength(1000);

            entity.Property(p => p.ImageUrl)
                  .HasMaxLength(500);

            entity.Property(p => p.Tag)
                  .HasConversion<string>()
                  .HasMaxLength(50);

            entity.Property(p => p.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(p => p.UpvotesCount).HasDefaultValue(0);
            entity.Property(p => p.DownvotesCount).HasDefaultValue(0);
            entity.Property(p => p.Score).HasDefaultValue(0);
        });

        // ── User ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Username)
                  .IsRequired()
                  .HasMaxLength(50);
            
            entity.HasIndex(u => u.Username).IsUnique();

            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(20);
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Seed Admin (Password: admin123)
            // BCrypt Hash for "admin123" (Cost: 11)
            entity.HasData(new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "$2a$11$AyFXkt03pFpbJmTpNxVL/O5Gf29bZJHGmr6LzMGb9ZyIns7u4Z2VO",
                Role = "Admin",
                CreatedAt = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc)
            });
        });

        // ── Comment ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(c => c.AuthorName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(c => c.Post)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(c => c.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Vote ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(v => v.Id);

            // Store enum as string
            entity.Property(v => v.VoteType)
                  .HasConversion<string>()
                  .HasMaxLength(10);

            entity.Property(v => v.VoterToken)
                  .IsRequired()
                  .HasMaxLength(200);

            // Unique constraint: 1 token can only vote once per post
            entity.HasIndex(v => new { v.PostId, v.VoterToken })
                  .IsUnique();

            entity.HasOne(v => v.Post)
                  .WithMany(p => p.Votes)
                  .HasForeignKey(v => v.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Seed Data (38 địa điểm thật) ─────────────────────────────────────
        // "city" từ JSON không có trong entity Post, nên được gắn vào cuối Description.
        // CreatedAt trải đều trong 30 ngày gần nhất (2026-05-27 → 2026-06-25) UTC.
        modelBuilder.Entity<Post>().HasData(

            // ── FOOD ──────────────────────────────────────────────────────────
            new Post
            {
                Id          = 1,
                Title       = "Bánh đúc nóng Lê Ngọc Hân",
                Description = "Quán nhỏ nằm trong nhà ở phố Lê Ngọc Hân, bán món bánh đúc gia truyền lâu năm cùng bún, miến, bánh đa. Không gian đơn giản nhưng luôn đông khách ăn tại chỗ. — Hà Nội",
                Tag         = PostTag.Food,
                Latitude    = 21.0166,
                Longitude   = 105.8540,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 5, 27, 8, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 2,
                Title       = "Phở \"bưng\" Hàng Trống",
                Description = "Quán phở không biển hiệu, không bàn ghế, khách phải tự bưng bát ra ngồi vỉa hè ăn. Mở từ chiều, nước dùng đậm đà, là một trong những quán phở 'ẩn' đặc trưng của phố cổ. — Hà Nội",
                Tag         = PostTag.Food,
                Latitude    = 21.0285,
                Longitude   = 105.8500,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 5, 28, 9, 15, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 3,
                Title       = "Sứa đỏ mắm tôm Hàng Chiếu",
                Description = "Gánh hàng nhỏ bán sứa đỏ chấm mắm tôm tồn tại hơn 80 năm, chế biến trong ngõ rồi mang ra bán. Món ăn vặt đặc trưng mùa hè của người Hà Nội. — Hà Nội",
                Tag         = PostTag.Food,
                Latitude    = 21.0345,
                Longitude   = 105.8505,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 5, 28, 14, 20, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 4,
                Title       = "Bún ốc Cô Giang",
                Description = "Gánh bún ốc cổ truyền ngồi vỉa hè phố Lương Ngọc Quyến, chỉ đơn giản bún, ốc, cà chua, hành lá và nước dùng chua dịu từ dấm bỗng. — Hà Nội",
                Tag         = PostTag.Food,
                Latitude    = 21.0310,
                Longitude   = 105.8520,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 5, 29, 7, 45, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 5,
                Title       = "Bò lá lốt hẻm Phan Văn Hân",
                Description = "Quán nằm sâu trong hẻm ẩm thực Phan Văn Hân, chuyên món bò cuốn lá lốt nướng than thơm nồng ngay từ đầu hẻm. Giá bình dân, chủ yếu khách quen địa phương. — TP. Hồ Chí Minh",
                Tag         = PostTag.Food,
                Latitude    = 10.8030,
                Longitude   = 106.7090,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 5, 30, 11, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 6,
                Title       = "Bánh mì chảo Hòa Mã",
                Description = "Tiệm bánh mì chảo lâu đời hơn nửa thế kỷ, nằm khiêm tốn trong hẻm nhỏ nhưng luôn đông khách buổi sáng. Phần ăn đầy đặn với trứng, chả, pate, xúc xích. — TP. Hồ Chí Minh",
                Tag         = PostTag.Food,
                Latitude    = 10.7820,
                Longitude   = 106.6900,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 5, 31, 8, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 7,
                Title       = "Cơm tấm Ba Ghiền",
                Description = "Một trong những địa chỉ cơm tấm sườn bì chả được người Sài Gòn lâu năm tin chọn, nước mắm pha theo công thức riêng. — TP. Hồ Chí Minh",
                Tag         = PostTag.Food,
                Latitude    = 10.7990,
                Longitude   = 106.6800,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 1, 10, 15, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 8,
                Title       = "Hủ tiếu gõ đêm Sài Gòn",
                Description = "Văn hóa ăn khuya đặc trưng của Sài Gòn: xe hủ tiếu nhỏ đẩy dọc hẻm phố, báo hiệu bằng tiếng gõ thanh gỗ thay cho rao hàng. — TP. Hồ Chí Minh",
                Tag         = PostTag.Food,
                Latitude    = 10.7670,
                Longitude   = 106.6920,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 1, 18, 40, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 9,
                Title       = "Bún bò Mệ Kéo",
                Description = "Gánh bún bò gia truyền hơn 70 năm trên đường Bạch Đằng, chỉ bán buổi sáng và thường hết hàng trước 10 giờ. Đặc biệt là không có thịt bò mà thay bằng chả cua, huyết, ba chỉ. — Huế",
                Tag         = PostTag.Food,
                Latitude    = 16.4674,
                Longitude   = 107.5905,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 2, 7, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 10,
                Title       = "Bánh bèo, nậm, lọc Bà Đỏ",
                Description = "Quán bánh lâu năm phục vụ trọn bộ đặc sản bánh Huế: bánh bèo, bánh nậm, bánh lọc, mỗi loại mang hương vị riêng nhưng đều đậm chất cố đô. — Huế",
                Tag         = PostTag.Food,
                Latitude    = 16.4610,
                Longitude   = 107.5870,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 3, 9, 20, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 11,
                Title       = "Cơm hến Cồn Hến",
                Description = "Món cơm hến trộn dân dã của Huế, nguyên liệu hến tươi lấy ngay từ Cồn Hến giữa sông Hương, ăn cùng nước hến nóng và mắm ruốc. — Huế",
                Tag         = PostTag.Food,
                Latitude    = 16.4730,
                Longitude   = 107.6050,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 3, 15, 10, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 12,
                Title       = "Cao lầu giếng Bá Lễ",
                Description = "Món cao lầu trứ danh phố Hội, theo cách làm truyền thống dùng nước từ giếng cổ Bá Lễ để tạo độ dai đặc trưng cho sợi mì. — Hội An",
                Tag         = PostTag.Food,
                Latitude    = 15.8801,
                Longitude   = 108.3380,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 4, 8, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 13,
                Title       = "Bún riêu chợ nổi Cái Răng",
                Description = "Ăn bún riêu ngay trên thuyền giữa chợ nổi Cái Răng vào sáng sớm, vừa thưởng thức món ăn vừa ngắm cảnh ghe thuyền tấp nập. — Cần Thơ",
                Tag         = PostTag.Food,
                Latitude    = 10.0050,
                Longitude   = 105.7470,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 5, 6, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 14,
                Title       = "Pizza hủ tiếu lò Sáu Hoài",
                Description = "Món hủ tiếu chiên giòn phủ trứng, thịt khìa và đậu phộng, được dân địa phương gọi vui là 'pizza' kiểu miền Tây, bán gần lò hủ tiếu truyền thống. — Cần Thơ",
                Tag         = PostTag.Food,
                Latitude    = 10.0080,
                Longitude   = 105.7490,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 5, 14, 45, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 15,
                Title       = "Lẩu vịt nấu chao hẻm Lý Tự Trọng",
                Description = "Khu hẻm tập trung nhiều quán lẩu vịt nấu chao lâu năm, vị béo đặc trưng của chao kết hợp thịt vịt mềm, ăn cùng rau và bún. — Cần Thơ",
                Tag         = PostTag.Food,
                Latitude    = 10.0310,
                Longitude   = 105.7740,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 6, 9, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 16,
                Title       = "Bánh ướt lòng gà & bánh căn Bình Minh Ơi",
                Description = "Quán cà phê phố núi phục vụ cả ngày, nổi tiếng với hai món đặc sản Đà Lạt: bánh ướt lòng gà thơm và bánh căn nóng giòn. — Đà Lạt",
                Tag         = PostTag.Food,
                Latitude    = 11.9520,
                Longitude   = 108.4350,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 7, 8, 30, 0, DateTimeKind.Utc)
            },

            // ── COFFEE ────────────────────────────────────────────────────────
            new Post
            {
                Id          = 17,
                Title       = "Tranquil Books & Coffee",
                Description = "Quán cà phê sách nằm trong ngõ nhỏ giữa phố cổ, không gian châu Âu hoài cổ, lý tưởng để đọc sách và làm việc yên tĩnh. — Hà Nội",
                Tag         = PostTag.Coffee,
                Latitude    = 21.0320,
                Longitude   = 105.8500,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 8, 10, 20, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 18,
                Title       = "Aerie Coffee",
                Description = "Quán cà phê ẩn trong khu tập thể cũ trên phố Hồng Hà, phải leo cầu thang hẹp mới lên được. Ban công xanh mát nhìn xuống phố nhưng vẫn giữ được sự yên tĩnh hiếm có. — Hà Nội",
                Tag         = PostTag.Coffee,
                Latitude    = 21.0420,
                Longitude   = 105.8550,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 9, 7, 15, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 19,
                Title       = "Tổ Chim Xanh",
                Description = "Quán cà phê sách nằm trong khu tập thể cũ ở phố Bà Triệu, có sân thượng thoáng và hay tổ chức giao lưu sách, phim. — Hà Nội",
                Tag         = PostTag.Coffee,
                Latitude    = 21.0167,
                Longitude   = 105.8508,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 9, 20, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 20,
                Title       = "Xofa Cafe & Bistro",
                Description = "Quán cà phê phong cách Pháp cổ nằm trong ngõ nhỏ, mở 24/24, nội thất sofa êm ái và kệ sách phong phú. — Hà Nội",
                Tag         = PostTag.Coffee,
                Latitude    = 21.0245,
                Longitude   = 105.8580,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 10, 9, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 21,
                Title       = "Hoi An Coffee Hub",
                Description = "Quán cà phê vườn nằm sâu trong hẻm yên tĩnh, thiết kế từ gỗ lũa mộc mạc, không gian xanh mát tách biệt khỏi phố cổ ồn ào. — Hội An",
                Tag         = PostTag.Coffee,
                Latitude    = 15.8780,
                Longitude   = 108.3260,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 11, 8, 45, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 22,
                Title       = "Reaching Out Teahouse",
                Description = "Quán trà do nhân viên khiếm thính phục vụ, không gian tĩnh lặng tuyệt đối, khách được khuyến khích giao tiếp bằng giấy bút hoặc ra dấu. — Hội An",
                Tag         = PostTag.Coffee,
                Latitude    = 15.8794,
                Longitude   = 108.3260,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 12, 10, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 23,
                Title       = "Roving Chillhouse",
                Description = "Quán cà phê đồng quê giữa cánh đồng lúa Cẩm Hà, không gian mở bốn bề ruộng xanh, cách xa sự ồn ào của trung tâm phố cổ. — Hội An",
                Tag         = PostTag.Coffee,
                Latitude    = 15.9020,
                Longitude   = 108.3150,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 12, 15, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 24,
                Title       = "Túi Mơ To",
                Description = "Quán cà phê kết hợp nhà hàng và homestay gần núi Langbiang, nổi bật với vườn hoa và view nhìn thẳng ra đỉnh Hòn Bồ. — Đà Lạt",
                Tag         = PostTag.Coffee,
                Latitude    = 11.9460,
                Longitude   = 108.4420,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 13, 8, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 25,
                Title       = "Mê Linh Coffee Garden",
                Description = "Quán cà phê nằm giữa đồi chè và vườn cà phê chồn, tầm nhìn 360 độ ra cao nguyên, nổi tiếng với cà phê chồn nguyên chất. — Đà Lạt",
                Tag         = PostTag.Coffee,
                Latitude    = 11.9100,
                Longitude   = 108.4900,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 14, 9, 15, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 26,
                Title       = "Cà Phê Tùng",
                Description = "Quán cà phê lâu đời giữa trung tâm Đà Lạt, giữ nguyên nội thất cổ và nhạc Trịnh, phù hợp cho ai yêu không khí hoài niệm. — Đà Lạt",
                Tag         = PostTag.Coffee,
                Latitude    = 11.9404,
                Longitude   = 108.4380,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 15, 11, 0, 0, DateTimeKind.Utc)
            },

            // ── SCENIC ROUTE ──────────────────────────────────────────────────
            new Post
            {
                Id          = 27,
                Title       = "Đèo Hải Vân",
                Description = "Cung đường ven biển dài khoảng 21km nối Đà Nẵng và Huế, nổi tiếng với hơn 100 khúc cua và view toàn cảnh vịnh Lăng Cô, bán đảo Sơn Trà. — Đà Nẵng - Huế",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 16.2090,
                Longitude   = 108.1170,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 16, 7, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 28,
                Title       = "Hà Giang Loop - Mã Pí Lèng",
                Description = "Một trong tứ đại đỉnh đèo Việt Nam, ôm trọn dòng sông Nho Quế xanh ngọc, đi qua cao nguyên đá Đồng Văn hùng vĩ. — Hà Giang",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 23.2750,
                Longitude   = 105.4100,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 17, 8, 45, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 29,
                Title       = "Sài Gòn - Vũng Tàu - Phan Thiết - Mũi Né",
                Description = "Cung đường ven biển dài khoảng 250km, đi qua bãi biển Vũng Tàu, hải đăng Kê Gà, và những đồi cát Mũi Né. — Bình Thuận",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 10.9330,
                Longitude   = 108.2880,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 18, 10, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 30,
                Title       = "Sài Gòn - Đà Lạt",
                Description = "Cung đường phượt khoảng 300km với những khúc cua tay áo lên cao nguyên Lâm Đồng, kết thúc ở thành phố sương mù Đà Lạt. — Lâm Đồng",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 11.5470,
                Longitude   = 107.8050,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 19, 9, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 31,
                Title       = "Nha Trang - Đà Lạt",
                Description = "Cung đường đèo nối thành phố biển và thành phố cao nguyên, đi qua những vách núi hùng vĩ và cánh đồng hoa trên đường lên đèo Khánh Lê. — Khánh Hòa - Lâm Đồng",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 12.0500,
                Longitude   = 108.5980,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 20, 8, 15, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 32,
                Title       = "Vịnh Vĩnh Hy - Ninh Chữ - Bình Lập",
                Description = "Cung đường ven biển từ Ninh Chữ đến Bình Lập, đi qua đồng muối, vườn nho Thái An và vịnh Vĩnh Hy nước trong xanh. — Ninh Thuận - Khánh Hòa",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 11.6850,
                Longitude   = 109.1480,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 21, 10, 45, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 33,
                Title       = "Quy Nhơn - Phú Yên (Ghềnh Đá Đĩa)",
                Description = "Cung đường ven biển miền Trung đi qua Eo Gió, Kỳ Co và ghềnh đá xếp tầng độc đáo Ghềnh Đá Đĩa. — Bình Định - Phú Yên",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 13.4060,
                Longitude   = 109.2460,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 22, 9, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 34,
                Title       = "Đèo Cả",
                Description = "Con đèo nối hai tỉnh Phú Yên và Khánh Hòa, một bên là núi xanh, một bên là biển xanh trải dài đến chân trời. — Phú Yên - Khánh Hòa",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 12.9330,
                Longitude   = 109.3270,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 23, 8, 30, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 35,
                Title       = "Sapa - Mù Cang Chải qua đèo Pha Đin",
                Description = "Cung đường Tây Bắc đi qua ruộng bậc thang Mù Cang Chải và đèo Pha Đin nổi tiếng, đẹp nhất vào mùa lúa chín tháng 9-10. — Lào Cai - Yên Bái - Điện Biên",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 21.5870,
                Longitude   = 103.5170,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 23, 16, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 36,
                Title       = "Đà Nẵng - Lăng Cô ven biển",
                Description = "Cung đường biển khoảng 70km từ Đà Nẵng đến vịnh Lăng Cô, một trong những vịnh biển được đánh giá đẹp nhất Việt Nam. — Đà Nẵng - Thừa Thiên Huế",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 16.2390,
                Longitude   = 108.0570,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 24, 9, 15, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 37,
                Title       = "Phan Thiết - Bình Thuận - Lâm Đồng",
                Description = "Cung đường khoảng 250km từ vùng biển cát trắng Bình Thuận lên những con đèo uốn lượn của cao nguyên Lâm Đồng. — Bình Thuận - Lâm Đồng",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 11.5230,
                Longitude   = 108.3300,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 25, 8, 0, 0, DateTimeKind.Utc)
            },
            new Post
            {
                Id          = 38,
                Title       = "Phá Tam Giang",
                Description = "Đầm phá được ví như 'vịnh Hạ Long trên cạn' nằm trên cung đường ven biển Huế - Đà Nẵng, nổi bật với cồn cát trắng và rừng nguyên sinh. — Huế",
                Tag         = PostTag.ScenicRoute,
                Latitude    = 16.4500,
                Longitude   = 107.6800,
                ImageUrl    = null,
                CreatedAt   = new DateTime(2026, 6, 25, 14, 30, 0, DateTimeKind.Utc)
            },

            // ── HOTEL ────────────────────────────────────────────────────────
            new Post
            {
                Id             = 39,
                Title          = "Homestay Bái Đính Ninh Bình",
                Description    = "",
                Tag            = PostTag.Hotel,
                Latitude       = 20.2626,
                Longitude      = 105.8613,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },
            new Post
            {
                Id             = 40,
                Title          = "Khách sạn Mường Thanh Đà Nẵng",
                Description    = "",
                Tag            = PostTag.Hotel,
                Latitude       = 16.0544,
                Longitude      = 108.2022,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },
            new Post
            {
                Id             = 41,
                Title          = "Resort Ana Mandara Nha Trang",
                Description    = "",
                Tag            = PostTag.Hotel,
                Latitude       = 12.2388,
                Longitude      = 109.1967,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },
            new Post
            {
                Id             = 42,
                Title          = "Homestay Cô Tô Quảng Ninh",
                Description    = "",
                Tag            = PostTag.Hotel,
                Latitude       = 21.0558,
                Longitude      = 107.7630,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },

            // ── ATTRACTION ───────────────────────────────────────────────────
            new Post
            {
                Id             = 43,
                Title          = "Phố cổ Hội An",
                Description    = "",
                Tag            = PostTag.Attraction,
                Latitude       = 15.8801,
                Longitude      = 108.3380,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },
            new Post
            {
                Id             = 44,
                Title          = "Vịnh Hạ Long",
                Description    = "",
                Tag            = PostTag.Attraction,
                Latitude       = 20.9101,
                Longitude      = 107.1839,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },
            new Post
            {
                Id             = 45,
                Title          = "Hoàng Thành Huế",
                Description    = "",
                Tag            = PostTag.Attraction,
                Latitude       = 16.4698,
                Longitude      = 107.5784,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            },
            new Post
            {
                Id             = 46,
                Title          = "Thánh địa Mỹ Sơn",
                Description    = "",
                Tag            = PostTag.Attraction,
                Latitude       = 15.7644,
                Longitude      = 108.1169,
                ImageUrl       = null,
                CreatedAt      = new DateTime(2026, 6, 25),
                UpvotesCount   = 0,
                DownvotesCount = 0,
                Score          = 0
            }
        );
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocalVibe.API.Migrations
{
    /// <inheritdoc />
    public partial class OptimizeDatabaseAndSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpvotesCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DownvotesCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Score = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    VoteType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VoterToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Latitude", "Longitude", "Tag", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 27, 8, 30, 0, 0, DateTimeKind.Utc), "Quán nhỏ nằm trong nhà ở phố Lê Ngọc Hân, bán món bánh đúc gia truyền lâu năm cùng bún, miến, bánh đa. Không gian đơn giản nhưng luôn đông khách ăn tại chỗ. — Hà Nội", null, 21.0166, 105.854, "Food", "Bánh đúc nóng Lê Ngọc Hân" },
                    { 2, new DateTime(2026, 5, 28, 9, 15, 0, 0, DateTimeKind.Utc), "Quán phở không biển hiệu, không bàn ghế, khách phải tự bưng bát ra ngồi vỉa hè ăn. Mở từ chiều, nước dùng đậm đà, là một trong những quán phở 'ẩn' đặc trưng của phố cổ. — Hà Nội", null, 21.028500000000001, 105.84999999999999, "Food", "Phở \"bưng\" Hàng Trống" },
                    { 3, new DateTime(2026, 5, 28, 14, 20, 0, 0, DateTimeKind.Utc), "Gánh hàng nhỏ bán sứa đỏ chấm mắm tôm tồn tại hơn 80 năm, chế biến trong ngõ rồi mang ra bán. Món ăn vặt đặc trưng mùa hè của người Hà Nội. — Hà Nội", null, 21.034500000000001, 105.8505, "Food", "Sứa đỏ mắm tôm Hàng Chiếu" },
                    { 4, new DateTime(2026, 5, 29, 7, 45, 0, 0, DateTimeKind.Utc), "Gánh bún ốc cổ truyền ngồi vỉa hè phố Lương Ngọc Quyến, chỉ đơn giản bún, ốc, cà chua, hành lá và nước dùng chua dịu từ dấm bỗng. — Hà Nội", null, 21.030999999999999, 105.852, "Food", "Bún ốc Cô Giang" },
                    { 5, new DateTime(2026, 5, 30, 11, 0, 0, 0, DateTimeKind.Utc), "Quán nằm sâu trong hẻm ẩm thực Phan Văn Hân, chuyên món bò cuốn lá lốt nướng than thơm nồng ngay từ đầu hẻm. Giá bình dân, chủ yếu khách quen địa phương. — TP. Hồ Chí Minh", null, 10.803000000000001, 106.709, "Food", "Bò lá lốt hẻm Phan Văn Hân" },
                    { 6, new DateTime(2026, 5, 31, 8, 30, 0, 0, DateTimeKind.Utc), "Tiệm bánh mì chảo lâu đời hơn nửa thế kỷ, nằm khiêm tốn trong hẻm nhỏ nhưng luôn đông khách buổi sáng. Phần ăn đầy đặn với trứng, chả, pate, xúc xích. — TP. Hồ Chí Minh", null, 10.782, 106.69, "Food", "Bánh mì chảo Hòa Mã" },
                    { 7, new DateTime(2026, 6, 1, 10, 15, 0, 0, DateTimeKind.Utc), "Một trong những địa chỉ cơm tấm sườn bì chả được người Sài Gòn lâu năm tin chọn, nước mắm pha theo công thức riêng. — TP. Hồ Chí Minh", null, 10.798999999999999, 106.68000000000001, "Food", "Cơm tấm Ba Ghiền" },
                    { 8, new DateTime(2026, 6, 1, 18, 40, 0, 0, DateTimeKind.Utc), "Văn hóa ăn khuya đặc trưng của Sài Gòn: xe hủ tiếu nhỏ đẩy dọc hẻm phố, báo hiệu bằng tiếng gõ thanh gỗ thay cho rao hàng. — TP. Hồ Chí Minh", null, 10.766999999999999, 106.69199999999999, "Food", "Hủ tiếu gõ đêm Sài Gòn" },
                    { 9, new DateTime(2026, 6, 2, 7, 30, 0, 0, DateTimeKind.Utc), "Gánh bún bò gia truyền hơn 70 năm trên đường Bạch Đằng, chỉ bán buổi sáng và thường hết hàng trước 10 giờ. Đặc biệt là không có thịt bò mà thay bằng chả cua, huyết, ba chỉ. — Huế", null, 16.467400000000001, 107.59050000000001, "Food", "Bún bò Mệ Kéo" },
                    { 10, new DateTime(2026, 6, 3, 9, 20, 0, 0, DateTimeKind.Utc), "Quán bánh lâu năm phục vụ trọn bộ đặc sản bánh Huế: bánh bèo, bánh nậm, bánh lọc, mỗi loại mang hương vị riêng nhưng đều đậm chất cố đô. — Huế", null, 16.460999999999999, 107.587, "Food", "Bánh bèo, nậm, lọc Bà Đỏ" },
                    { 11, new DateTime(2026, 6, 3, 15, 10, 0, 0, DateTimeKind.Utc), "Món cơm hến trộn dân dã của Huế, nguyên liệu hến tươi lấy ngay từ Cồn Hến giữa sông Hương, ăn cùng nước hến nóng và mắm ruốc. — Huế", null, 16.472999999999999, 107.605, "Food", "Cơm hến Cồn Hến" },
                    { 12, new DateTime(2026, 6, 4, 8, 0, 0, 0, DateTimeKind.Utc), "Món cao lầu trứ danh phố Hội, theo cách làm truyền thống dùng nước từ giếng cổ Bá Lễ để tạo độ dai đặc trưng cho sợi mì. — Hội An", null, 15.880100000000001, 108.33799999999999, "Food", "Cao lầu giếng Bá Lễ" },
                    { 13, new DateTime(2026, 6, 5, 6, 30, 0, 0, DateTimeKind.Utc), "Ăn bún riêu ngay trên thuyền giữa chợ nổi Cái Răng vào sáng sớm, vừa thưởng thức món ăn vừa ngắm cảnh ghe thuyền tấp nập. — Cần Thơ", null, 10.005000000000001, 105.747, "Food", "Bún riêu chợ nổi Cái Răng" },
                    { 14, new DateTime(2026, 6, 5, 14, 45, 0, 0, DateTimeKind.Utc), "Món hủ tiếu chiên giòn phủ trứng, thịt khìa và đậu phộng, được dân địa phương gọi vui là 'pizza' kiểu miền Tây, bán gần lò hủ tiếu truyền thống. — Cần Thơ", null, 10.007999999999999, 105.749, "Food", "Pizza hủ tiếu lò Sáu Hoài" },
                    { 15, new DateTime(2026, 6, 6, 9, 0, 0, 0, DateTimeKind.Utc), "Khu hẻm tập trung nhiều quán lẩu vịt nấu chao lâu năm, vị béo đặc trưng của chao kết hợp thịt vịt mềm, ăn cùng rau và bún. — Cần Thơ", null, 10.031000000000001, 105.774, "Food", "Lẩu vịt nấu chao hẻm Lý Tự Trọng" },
                    { 16, new DateTime(2026, 6, 7, 8, 30, 0, 0, DateTimeKind.Utc), "Quán cà phê phố núi phục vụ cả ngày, nổi tiếng với hai món đặc sản Đà Lạt: bánh ướt lòng gà thơm và bánh căn nóng giòn. — Đà Lạt", null, 11.952, 108.435, "Food", "Bánh ướt lòng gà & bánh căn Bình Minh Ơi" },
                    { 17, new DateTime(2026, 6, 8, 10, 20, 0, 0, DateTimeKind.Utc), "Quán cà phê sách nằm trong ngõ nhỏ giữa phố cổ, không gian châu Âu hoài cổ, lý tưởng để đọc sách và làm việc yên tĩnh. — Hà Nội", null, 21.032, 105.84999999999999, "Coffee", "Tranquil Books & Coffee" },
                    { 18, new DateTime(2026, 6, 9, 7, 15, 0, 0, DateTimeKind.Utc), "Quán cà phê ẩn trong khu tập thể cũ trên phố Hồng Hà, phải leo cầu thang hẹp mới lên được. Ban công xanh mát nhìn xuống phố nhưng vẫn giữ được sự yên tĩnh hiếm có. — Hà Nội", null, 21.042000000000002, 105.855, "Coffee", "Aerie Coffee" },
                    { 19, new DateTime(2026, 6, 9, 20, 30, 0, 0, DateTimeKind.Utc), "Quán cà phê sách nằm trong khu tập thể cũ ở phố Bà Triệu, có sân thượng thoáng và hay tổ chức giao lưu sách, phim. — Hà Nội", null, 21.0167, 105.85080000000001, "Coffee", "Tổ Chim Xanh" },
                    { 20, new DateTime(2026, 6, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Quán cà phê phong cách Pháp cổ nằm trong ngõ nhỏ, mở 24/24, nội thất sofa êm ái và kệ sách phong phú. — Hà Nội", null, 21.0245, 105.858, "Coffee", "Xofa Cafe & Bistro" },
                    { 21, new DateTime(2026, 6, 11, 8, 45, 0, 0, DateTimeKind.Utc), "Quán cà phê vườn nằm sâu trong hẻm yên tĩnh, thiết kế từ gỗ lũa mộc mạc, không gian xanh mát tách biệt khỏi phố cổ ồn ào. — Hội An", null, 15.878, 108.32599999999999, "Coffee", "Hoi An Coffee Hub" },
                    { 22, new DateTime(2026, 6, 12, 10, 30, 0, 0, DateTimeKind.Utc), "Quán trà do nhân viên khiếm thính phục vụ, không gian tĩnh lặng tuyệt đối, khách được khuyến khích giao tiếp bằng giấy bút hoặc ra dấu. — Hội An", null, 15.8794, 108.32599999999999, "Coffee", "Reaching Out Teahouse" },
                    { 23, new DateTime(2026, 6, 12, 15, 0, 0, 0, DateTimeKind.Utc), "Quán cà phê đồng quê giữa cánh đồng lúa Cẩm Hà, không gian mở bốn bề ruộng xanh, cách xa sự ồn ào của trung tâm phố cổ. — Hội An", null, 15.901999999999999, 108.315, "Coffee", "Roving Chillhouse" },
                    { 24, new DateTime(2026, 6, 13, 8, 0, 0, 0, DateTimeKind.Utc), "Quán cà phê kết hợp nhà hàng và homestay gần núi Langbiang, nổi bật với vườn hoa và view nhìn thẳng ra đỉnh Hòn Bồ. — Đà Lạt", null, 11.946, 108.44199999999999, "Coffee", "Túi Mơ To" },
                    { 25, new DateTime(2026, 6, 14, 9, 15, 0, 0, DateTimeKind.Utc), "Quán cà phê nằm giữa đồi chè và vườn cà phê chồn, tầm nhìn 360 độ ra cao nguyên, nổi tiếng với cà phê chồn nguyên chất. — Đà Lạt", null, 11.91, 108.48999999999999, "Coffee", "Mê Linh Coffee Garden" },
                    { 26, new DateTime(2026, 6, 15, 11, 0, 0, 0, DateTimeKind.Utc), "Quán cà phê lâu đời giữa trung tâm Đà Lạt, giữ nguyên nội thất cổ và nhạc Trịnh, phù hợp cho ai yêu không khí hoài niệm. — Đà Lạt", null, 11.9404, 108.438, "Coffee", "Cà Phê Tùng" },
                    { 27, new DateTime(2026, 6, 16, 7, 30, 0, 0, DateTimeKind.Utc), "Cung đường ven biển dài khoảng 21km nối Đà Nẵng và Huế, nổi tiếng với hơn 100 khúc cua và view toàn cảnh vịnh Lăng Cô, bán đảo Sơn Trà. — Đà Nẵng - Huế", null, 16.209, 108.117, "ScenicRoute", "Đèo Hải Vân" },
                    { 28, new DateTime(2026, 6, 17, 8, 45, 0, 0, DateTimeKind.Utc), "Một trong tứ đại đỉnh đèo Việt Nam, ôm trọn dòng sông Nho Quế xanh ngọc, đi qua cao nguyên đá Đồng Văn hùng vĩ. — Hà Giang", null, 23.274999999999999, 105.41, "ScenicRoute", "Hà Giang Loop - Mã Pí Lèng" },
                    { 29, new DateTime(2026, 6, 18, 10, 0, 0, 0, DateTimeKind.Utc), "Cung đường ven biển dài khoảng 250km, đi qua bãi biển Vũng Tàu, hải đăng Kê Gà, và những đồi cát Mũi Né. — Bình Thuận", null, 10.933, 108.288, "ScenicRoute", "Sài Gòn - Vũng Tàu - Phan Thiết - Mũi Né" },
                    { 30, new DateTime(2026, 6, 19, 9, 30, 0, 0, DateTimeKind.Utc), "Cung đường phượt khoảng 300km với những khúc cua tay áo lên cao nguyên Lâm Đồng, kết thúc ở thành phố sương mù Đà Lạt. — Lâm Đồng", null, 11.547000000000001, 107.80500000000001, "ScenicRoute", "Sài Gòn - Đà Lạt" },
                    { 31, new DateTime(2026, 6, 20, 8, 15, 0, 0, DateTimeKind.Utc), "Cung đường đèo nối thành phố biển và thành phố cao nguyên, đi qua những vách núi hùng vĩ và cánh đồng hoa trên đường lên đèo Khánh Lê. — Khánh Hòa - Lâm Đồng", null, 12.050000000000001, 108.598, "ScenicRoute", "Nha Trang - Đà Lạt" },
                    { 32, new DateTime(2026, 6, 21, 10, 45, 0, 0, DateTimeKind.Utc), "Cung đường ven biển từ Ninh Chữ đến Bình Lập, đi qua đồng muối, vườn nho Thái An và vịnh Vĩnh Hy nước trong xanh. — Ninh Thuận - Khánh Hòa", null, 11.685, 109.148, "ScenicRoute", "Vịnh Vĩnh Hy - Ninh Chữ - Bình Lập" },
                    { 33, new DateTime(2026, 6, 22, 9, 0, 0, 0, DateTimeKind.Utc), "Cung đường ven biển miền Trung đi qua Eo Gió, Kỳ Co và ghềnh đá xếp tầng độc đáo Ghềnh Đá Đĩa. — Bình Định - Phú Yên", null, 13.406000000000001, 109.246, "ScenicRoute", "Quy Nhơn - Phú Yên (Ghềnh Đá Đĩa)" },
                    { 34, new DateTime(2026, 6, 23, 8, 30, 0, 0, DateTimeKind.Utc), "Con đèo nối hai tỉnh Phú Yên và Khánh Hòa, một bên là núi xanh, một bên là biển xanh trải dài đến chân trời. — Phú Yên - Khánh Hòa", null, 12.933, 109.327, "ScenicRoute", "Đèo Cả" },
                    { 35, new DateTime(2026, 6, 23, 16, 0, 0, 0, DateTimeKind.Utc), "Cung đường Tây Bắc đi qua ruộng bậc thang Mù Cang Chải và đèo Pha Đin nổi tiếng, đẹp nhất vào mùa lúa chín tháng 9-10. — Lào Cai - Yên Bái - Điện Biên", null, 21.587, 103.517, "ScenicRoute", "Sapa - Mù Cang Chải qua đèo Pha Đin" },
                    { 36, new DateTime(2026, 6, 24, 9, 15, 0, 0, DateTimeKind.Utc), "Cung đường biển khoảng 70km từ Đà Nẵng đến vịnh Lăng Cô, một trong những vịnh biển được đánh giá đẹp nhất Việt Nam. — Đà Nẵng - Thừa Thiên Huế", null, 16.239000000000001, 108.057, "ScenicRoute", "Đà Nẵng - Lăng Cô ven biển" },
                    { 37, new DateTime(2026, 6, 25, 8, 0, 0, 0, DateTimeKind.Utc), "Cung đường khoảng 250km từ vùng biển cát trắng Bình Thuận lên những con đèo uốn lượn của cao nguyên Lâm Đồng. — Bình Thuận - Lâm Đồng", null, 11.523, 108.33, "ScenicRoute", "Phan Thiết - Bình Thuận - Lâm Đồng" },
                    { 38, new DateTime(2026, 6, 25, 14, 30, 0, 0, DateTimeKind.Utc), "Đầm phá được ví như 'vịnh Hạ Long trên cạn' nằm trên cung đường ven biển Huế - Đà Nẵng, nổi bật với cồn cát trắng và rừng nguyên sinh. — Huế", null, 16.449999999999999, 107.68000000000001, "ScenicRoute", "Phá Tam Giang" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$AyFXkt03pFpbJmTpNxVL/O5Gf29bZJHGmr6LzMGb9ZyIns7u4Z2VO", "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId_VoterToken",
                table: "Votes",
                columns: new[] { "PostId", "VoterToken" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}

using System;
using api.Authorization;
using api.Config;
using api.Config.Db;
using api.Exceptions;
using api.Extension;
using api.Fliter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using shared.Utils;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yitter.IdGenerator;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.Configure<MongodbSettings>(
    builder.Configuration.GetSection("Mongodbs"));

// Add services to the container.
builder.Services.AddBusiness();

// 雪花算法配置
var options = new IdGeneratorOptions(6);
YitIdHelper.SetIdGenerator(options);

builder.Services.AddControllers(opt =>
{
    // 统一设置路由前缀
    opt.UseCentralRoutePrefix(new RouteAttribute(configuration["context-path"]));
});

// 启动访问上下文 同理也可以使用自定义接口token获取对应上下文参数
builder.Services.AddHttpContextAccessor();

// 注册筛选器
builder.Services.Configure<MvcOptions>(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();

});

#region 配置登录认证、授权

// 配置登录认证
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //是否验证Issuer
        ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
        ValidateAudience = true, //是否验证Audience
        ValidAudience = configuration["Jwt:Audience"], //订阅人Audience
        ValidateIssuerSigningKey = true, //是否验证SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
        ValidateLifetime = true, //是否验证失效时间
        ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
        RequireExpirationTime = true,
    };
    // 当token验证通过后（执行完 JwtBearerEvents.TokenValidated 后），
    // 是否将token存储在 Microsoft.AspNetCore.Authentication.AuthenticationProperties 中
    // 默认 true
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        //此处为权限验证失败后触发的事件
        OnChallenge = context =>
        {
            //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
            context.HandleResponse();

            //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
            var payload = JsonConvert.SerializeObject(new { Code = HttpCode.UNAUTHORIZED_CODE, Message = "很抱歉，您无权访问该；请登录！" });
            //自定义返回的数据类型
            context.Response.ContentType = "application/json";
            //自定义返回状态码，默认为401 我这里改成 200
            context.Response.StatusCode = StatusCodes.Status200OK;
            //输出Json数据结果
            context.Response.WriteAsync(payload);
            return Task.FromResult(0);
        }
    };
});
// 注入jwt
builder.Services.AddScoped<JwtUtil>();
// 添加授权操作
builder.Services.AddAuthorization();
// 注册自定义授权处理器
builder.Services.AddSingleton<IAuthorizationHandler, PermissionsAuthorizationHandler>();
// 自定义授权失败结果返回
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomizeAuthorizationMiddlewareResultHandler>();

#endregion


//统一返回时间格式,配置返回的时间类型数据格式
builder.Services.AddMvc().AddJsonOptions((options) => {
    options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

// 启用身份验证和授权中间件
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar la tuber�a de solicitud HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor predeterminado de HSTS es de 30 d�as. Para producci�n, puedes cambiarlo seg�n sea necesario.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Cambiar el controlador y la acci�n por defecto a Translation/Index.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Translation}/{action=Index}/{id?}");

app.Run();

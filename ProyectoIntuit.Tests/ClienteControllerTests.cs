using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ProyectoIntuit.Context;
using ProyectoIntuit.Controllers;
using ProyectoIntuit.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ProyectoIntuit.Tests
{
    public class ClienteControllerTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // BD aislada por test
                .Options;

            return new AppDbContext(options);
        }

        // ------------------------------
        // 🔧 Helper: Crear un Cliente válido
        // ------------------------------
        private Cliente CrearClienteValido()
        {
            return new Cliente
            {
                Nombre = "Facundo",
                Apellido = "Fabbi",
                RazonSocial = "Empresa SA",
                Cuit = "20-12345678-3",
                Telefono = "3512345678",
                Email = "facu@mail.com",
                FechaNacimiento = DateTime.Now.AddYears(-20),
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
        }

        // ------------------------------
        // 🧪 GET ALL
        // ------------------------------
        [Fact]
        public async Task GetAll_ReturnsAllClientes()
        {
            // ARRANGE
            var context = GetInMemoryContext();

            context.Cliente.Add(CrearClienteValido());
            context.Cliente.Add(CrearClienteValido());
            await context.SaveChangesAsync();

            var logger = new NullLogger<ClienteController>();
            var controller = new ClienteController(context, logger);

            // ACT
            var result = await controller.GetAll();

            // ASSERT
            var ok = Assert.IsType<ActionResult<IEnumerable<Cliente>>>(result);
            var lista = Assert.IsAssignableFrom<IEnumerable<Cliente>>(ok.Value);

            Assert.Equal(2, lista.Count());
        }

        // ------------------------------
        // 🧪 GET BY ID (NOT FOUND)
        // ------------------------------
        [Fact]
        public async Task GetCliente_NotFound_WhenDoesNotExist()
        {
            // ARRANGE
            var context = GetInMemoryContext();
            var logger = new NullLogger<ClienteController>();
            var controller = new ClienteController(context, logger);

            // ACT
            var result = await controller.GetCliente(999);

            // ASSERT
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // ------------------------------
        // 🧪 INSERT
        // ------------------------------
        [Fact]
        public async Task Insert_AddsClienteSuccessfully()
        {
            // ARRANGE
            var context = GetInMemoryContext();
            var logger = new NullLogger<ClienteController>();
            var controller = new ClienteController(context, logger);

            var nuevo = CrearClienteValido();

            // ACT
            var result = await controller.Insert(nuevo);

            // ASSERT
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var cliente = Assert.IsType<Cliente>(created.Value);

            Assert.Equal(nuevo.Nombre, cliente.Nombre);
            Assert.True(cliente.Id > 0);
        }

        // ------------------------------
        // 🧪 UPDATE
        // ------------------------------
        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccess()
        {
            // ARRANGE
            var context = GetInMemoryContext();
            var cliente = CrearClienteValido();

            context.Cliente.Add(cliente);
            await context.SaveChangesAsync();

            cliente.Nombre = "Nombre Actualizado";

            var logger = new NullLogger<ClienteController>();
            var controller = new ClienteController(context, logger);

            // ACT
            var result = await controller.Update(cliente.Id, cliente);

            // ASSERT
            Assert.IsType<NoContentResult>(result);
        }

        // ------------------------------
        // 🧪 DELETE NOT FOUND
        // ------------------------------
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            // ARRANGE
            var context = GetInMemoryContext();
            var logger = new NullLogger<ClienteController>();
            var controller = new ClienteController(context, logger);

            // ACT
            var result = await controller.Delete(12345);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

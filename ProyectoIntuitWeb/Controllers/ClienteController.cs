using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoIntuit.Models;
using ProyectoIntuitWeb.Models;
using System.Text;

namespace ProyectoIntuitWeb.Controllers
{
    public class ClienteController : Controller
    {

        private readonly HttpClient _httpClient;
        public ClienteController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44332/api");
        }
        public async Task<IActionResult> Index(string q) // q es el término de búsqueda
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                // obtener todo (como hacías)
                //var response = await _httpClient.GetAsync("/Cliente/GetAll");
                var response = await _httpClient.GetAsync("/api/Cliente/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var clientes = JsonConvert.DeserializeObject<IEnumerable<ClienteViewModel>>(content);
                    return View("Index", clientes);
                }
                return View(new List<ClienteViewModel>());
            }
            else
            {
                // cuando q tiene valor:
                var nombreEncode = Uri.EscapeDataString(q);
                var response = await _httpClient.GetAsync($"/api/Cliente/Search?nombre={nombreEncode}");

                // Si tu API devuelve una lista (recomendado) deserializa IEnumerable
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Si la API devuelve una lista:
                    try
                    {
                        var clientes = JsonConvert.DeserializeObject<IEnumerable<ClienteViewModel>>(content);
                        // Si tu API todavía devuelve 1 solo Cliente, envuelve en lista:
                        if (clientes == null)
                        {
                            var clienteUnico = JsonConvert.DeserializeObject<ClienteViewModel>(content);
                            if (clienteUnico != null)
                                clientes = new List<ClienteViewModel> { clienteUnico };
                            else
                                clientes = new List<ClienteViewModel>();
                        }
                        return View("Index", clientes);
                    }
                    catch (Exception)
                    {
                        // fallback por si devuelve un objeto único:
                        var clienteUnico = JsonConvert.DeserializeObject<ClienteViewModel>(content);
                        if (clienteUnico != null)
                            return View("Index", new List<ClienteViewModel> { clienteUnico });
                        return View(new List<ClienteViewModel>());
                    }
                }
                // si la API devolvió NotFound -> no hay resultados
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return View(new List<ClienteViewModel>());

                // error
                return View(new List<ClienteViewModel>());
            }
        }

        /*public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Cliente/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<IEnumerable<ClienteViewModel>>(content);


                return View("Index", clientes);
            }

            // Manejar el caso en que la solicitud HTTP no fue exitosa.
            return View(new List<ClienteViewModel>()); // Puedes mostrar una vista vacía o un mensaje de error.
        }*/



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Cliente/Insert", content);

                if (response.IsSuccessStatusCode)
                {
                    // Manejar el caso de creación exitosa.
                    return RedirectToAction("Index");
                }
                else
                {
                    // Manejar el caso de error en la solicitud POST, por ejemplo, mostrando un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al crear el Cliente.");
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {

            var response = await _httpClient.GetAsync($"/api/Cliente/Get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<ClienteViewModel>(content);

                // Devuelve la vista de edición con los detalles del cliente.
                return View(cliente);
            }
            else
            {
                // Manejar el caso de error al obtener los detalles del cliente.
                return RedirectToAction("Details"); // Redirige a la página de lista de cliente u otra acción apropiada.
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ClienteViewModel cliente)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Cliente/Update/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Manejar el caso de actualización exitosa, por ejemplo, redirigiendo a la página de detalles del Cliente.
                    return RedirectToAction("Index", new { id });
                }
                else
                {
                    // Manejar el caso de error en la solicitud PUT o POST, por ejemplo, mostrando un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al actualizar el Cliente.");
                }
            }

            // Si hay errores de validación, vuelve a mostrar el formulario de edición con los errores.
            return View(cliente);
        }


        public async Task<IActionResult> Details(int id)
        {

            var response = await _httpClient.GetAsync($"/api/Cliente/Get/{id}");


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<ClienteViewModel>(content);

                // Devuelve la vista de edición con los detalles del Cliente.
                return View(cliente);
            }
            else
            {
                // Manejar el caso de error al obtener los detalles del Cliente.
                return RedirectToAction("Details"); // Redirige a la página de lista de Cliente u otra acción apropiada.
            }
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Cliente/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Maneja el caso de eliminación exitosa, por ejemplo, redirigiendo a la página de lista de Clientes.
                return RedirectToAction("Index");
            }
            else
            {
                // Maneja el caso de error en la solicitud DELETE, por ejemplo, mostrando un mensaje de error.
                TempData["Error"] = "Error al eliminar el Cliente.";
                return RedirectToAction("Index");
            }
        }
    }

    
}

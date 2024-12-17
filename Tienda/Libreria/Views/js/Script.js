// Obtener productos de la base de datos mediante una API
async function obtenerProductos() {
    try {
        const response = await fetch('/api/productos'); // URL de tu API
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error al obtener los productos:', error);
        return [];
    }
}

// Carrito de compras (array)
let carrito = JSON.parse(localStorage.getItem('carrito')) || [];

// Función para agregar un producto al carrito
function agregarAlCarrito(codigoProducto) {
    const producto = productos.find(p => p.CodigoProducto === codigoProducto);

    if (producto) {
        const productoEnCarrito = carrito.find(p => p.CodigoProducto === codigoProducto);

        if (productoEnCarrito) {
            if (productoEnCarrito.cantidad < producto.DisponibilidadInventario) {
                productoEnCarrito.cantidad++;
            } else {
                alert('No hay suficiente inventario disponible');
                return;
            }
        } else {
            carrito.push({
                ...producto,
                cantidad: 1
            });
        }

        localStorage.setItem('carrito', JSON.stringify(carrito));
        alert(`${producto.NombreProducto} agregado al carrito`);
        mostrarCarrito();
    } else {
        alert('Producto no encontrado');
    }
}

// Función para mostrar el carrito en una vista dedicada
function mostrarVistaCarrito() {
    const carritoContainer = document.getElementById('carrito-container');
    carritoContainer.innerHTML = '';

    if (carrito.length === 0) {
        carritoContainer.innerHTML = '<p class="text-center">El carrito está vacío. ¡Agrega productos!</p>';
        return;
    }

    carrito.forEach((item, index) => {
        carritoContainer.innerHTML += `
            <div class="card mb-3 shadow-sm">
                <div class="row g-0">
                    <div class="col-md-4">
                        <img src="${item.ImagenProducto}" class="img-fluid rounded-start" alt="${item.NombreProducto}" style="height: 200px; object-fit: cover;">
                    </div>
                    <div class="col-md-8">
                        <div class="card-body">
                            <h5 class="card-title">${item.NombreProducto}</h5>
                            <p class="card-text">${item.Descripcion}</p>
                            <p class="card-text">Cantidad: ${item.cantidad}</p>
                            <p class="fw-bold">Precio Total: $${((item.Precio - item.Descuento) * item.cantidad).toFixed(2)}</p>
                            <button class="btn btn-danger" onclick="eliminarDelCarrito(${index})">Eliminar</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    });

    const total = carrito.reduce((sum, item) => sum + (item.Precio - item.Descuento) * item.cantidad, 0);
    carritoContainer.innerHTML += `<h4 class="text-end fw-bold mt-4">Total del Carrito: $${total.toFixed(2)}</h4>`;
}

// Función para eliminar un producto del carrito
function eliminarDelCarrito(index) {
    carrito.splice(index, 1);
    localStorage.setItem('carrito', JSON.stringify(carrito));
    mostrarVistaCarrito();
}

// Inicializar productos desde la API y mostrar en consola
let productos = [];
(async function init() {
    productos = await obtenerProductos();
    mostrarVistaCarrito();
})();

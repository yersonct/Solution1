async function mostrar() {}
document.addEventListener("DOMContentLoaded", function () {
    const botones = document.querySelectorAll(".menu-button");
    const contenedor = document.querySelector(".content");
    const titulo = document.querySelector(".titulo");
    const ElementoBusqueda = document.querySelector(".contenedor-busqueda");
    botones.forEach(boton => {
        boton.addEventListener("click", async function () {
            const botonId = boton.id;
            try {
                const contextResponse = await fetch("../json/context.json");
                if (!contextResponse.ok) throw new Error("Error al cargar context.json");

                const contextData = await contextResponse.json();
                const config = contextData.find(item => item.id === botonId);
                if (!config) {
                    contenedor.innerHTML = "<p>Contenido no encontrado para el botón.</p>";
                    return;
                }

                titulo.innerHTML = config.title;
                contenedor.innerHTML = config.contenedor;
                const campos = config.campos || [];
                ElementoBusqueda.innerHTML = config.busqueda; // Asignar el contenido de búsqueda

                await cargarTabla(config, campos);
                agregarBotonMostrarTodos(config.ruta);
                contenedor.innerHTML += config.formulario;
                configurarFormularioUnico(config);
                configurarFormularioRegistro(config);

            } catch (error) {
                contenedor.innerHTML = `<p>Error: ${error.message}</p>`;
            }
        });
    });
});

async function cargarTabla(config, campos) {
    async function mostrar() {
        try {
            console.log("URL del endpoint:", config.ruta);

            const response = await fetch(config.ruta);
            if (!response.ok) throw new Error('Error al cargar los datos desde ' + config.ruta);

            const data = await response.json();
            const registros = data["$values"] || data; // Ajuste aquí

            console.log("Datos recibidos:", registros);

            const cuerpo = document.querySelector(".cuerpo-1");
            
            cuerpo.innerHTML = "";
            if (Array.isArray(registros) && registros.length > 0) {
                registros.forEach(item => {
                    let row = "<tr>";
                    campos.forEach(campo => {
                        row += `<td>${item[campo] ?? ""}</td>`;
                    });
                    row += `
                        <td>
                            <button class="buttonPersonalizado eliminar" onclick="eliminarElemento('${config.ruta}', ${item.id})">Eliminar</button>
                            <button class="buttonPersonalizado actualizar" onclick="actualizarElemento('${config.ruta}', ${item.id})">Actualizar</button>
                               <button class="buttonPersonalizado EliminadorLogico" onclick="EliminadorLogico('${config.ruta}', ${item.id})">ELiminador logico</button>
                        </td>
                    </tr>`;
                    cuerpo.innerHTML += row;
                });
            cuerpo.innerHTML+= "</tbody></table>"
            } else {
                cuerpo.innerHTML = "<tr><td colspan='4'>No hay datos</td></tr>";
                cuerpo.innerHTML+= "</tbody></table>"
            }
        } catch (error) {
            console.error("Error cargando datos:", error);
            document.querySelector(".cuerpo-1").innerHTML = "<tr><td colspan='4'>Error al cargar los datos.</td></tr>";
            cuerpo.innerHTML+= "</tbody></table>"
        }
    }
    mostrar();
}

function agregarBotonMostrarTodos(endpoint) {
    const contenedor = document.querySelector(".content");
    contenedor.innerHTML += `<button class="bttAllItems" onclick="mostrarTodos('${endpoint}')">Mostrar todos los registros</button>`;
}

async function mostrarTodos(endpoint) {
    try {
        const response = await fetch(endpoint);
        if (!response.ok) throw new Error("No se pudo cargar los datos");
        const data = await response.json();
        const registros = data["$values"] || data; // También aquí
        mostrar();
        console.log("Todos los registros:", registros);
    } catch (error) {
        console.error("Error:", error);
    }
}

function configurarFormularioUnico(config) {
    const form = document.querySelector(".formulario-unico");
    if (form) {
        form.addEventListener("submit", async function (e) {
            e.preventDefault();
            const id = document.querySelector(".datoUnico").value;
            try {
                const response = await fetch(`${config.ruta}/${id}`);
                if (!response.ok) throw new Error("No se encontró el registro con ese ID");

                const data = await response.json();
                console.log("Datos obtenidos por ID:", data);
            } catch (error) {
                console.error("Error:", error);
            }
        });
    }
}

function configurarFormularioRegistro(config) {
    const form = document.querySelector(".formularios");
    if (form) {
        form.addEventListener("submit", async function (e) {
            e.preventDefault();
            const formData = new FormData(form);
            const data = {};
            
            formData.forEach((value, key) => {
                if (key === 'restrictiondate') {
                    data[key] = new Date(value).toISOString(); // Convierte a formato ISO
                } else if (key === 'id_client') {
                    data[key] = parseInt(value);
                } else {
                    data[key] = value;
                }
            });

            try {
                console.log(data);  // Verifica qué datos están siendo enviados

                const response = await fetch(config.ruta, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(data)
                });

                const result = await response.json();
                alert(response.ok ? "Guardado exitosamente" : `Error: ${result.message}`);
            } catch (error) {
                alert("Error de red: " + error.message);
            }
        });
    }
}


async function eliminarElemento(endpoint, id) {
    if (!confirm("¿Estás seguro de que deseas eliminar este registro?")) return;

    try {
        const response = await fetch(`${endpoint}/${id}`, { method: "DELETE" });
        const result = await response.json();
        alert(response.ok ? "Eliminado correctamente" : `Error: ${result.message}`);
    } catch (error) {
        alert("Error eliminando: " + error.message);
    }
}

async function actualizarElemento(endpoint, id) {
    const nuevosDatos = prompt("Introduce los nuevos datos en formato JSON:");
    if (!nuevosDatos) return;

    try {
        const parsed = JSON.parse(nuevosDatos);
        const response = await fetch(`${endpoint}/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(parsed)
        });

        const result = await response.json();
        alert(response.ok ? "Actualizado correctamente" : `Error: ${result.message}`);
    } catch (error) {
        alert("Error actualizando: " + error.message);
    }
}

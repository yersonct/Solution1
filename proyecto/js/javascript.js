document.addEventListener("DOMContentLoaded", function () {
    const botones = document.querySelectorAll(".menu-button");
    const contenedor = document.querySelector(".content");
    const titulo = document.querySelector(".titulo");

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
                    </td>
                </tr>`;
                cuerpo.innerHTML += row;
            });
        } else {
            cuerpo.innerHTML = "<tr><td colspan='4'>No hay datos</td></tr>";
        }
    } catch (error) {
        console.error("Error cargando datos:", error);
        document.querySelector(".cuerpo-1").innerHTML = "<tr><td colspan='4'>Error al cargar los datos.</td></tr>";
    }
}

function agregarBotonMostrarTodos(endpoint) {
    const contenedor = document.querySelector(".content");
    contenedor.innerHTML += `<button onclick="mostrarTodos('${endpoint}')">Mostrar todos los registros</button>`;
}

async function mostrarTodos(endpoint) {
    try {
        const response = await fetch(endpoint);
        if (!response.ok) throw new Error("No se pudo cargar los datos");
        const data = await response.json();
        const registros = data["$values"] || data; // También aquí

        console.log("Todos los registros:", registros);
    } catch (error) {
        console.error("Error:", error);
    }
}

function configurarFormularioUnico(config) {
    const form = document.getElementById("formulario-unico");
    if (form) {
        form.addEventListener("submit", async function (e) {
            e.preventDefault();
            const id = document.getElementById("datoUnico").value;
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
    const form = document.getElementById(config.formId);
    if (form) {
        form.addEventListener("submit", async function (e) {
            e.preventDefault();
            const formData = new FormData(form);
            const data = {};
            formData.forEach((value, key) => {
                data[key] = isNaN(value) ? value : parseInt(value);
            });

            try {
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

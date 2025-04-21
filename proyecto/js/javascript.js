document.addEventListener("DOMContentLoaded", function () {
    const botones = document.querySelectorAll(".menu-button");
    const contenedor = document.querySelector(".content");
    const titulo = document.querySelector(".titulo");

    botones.forEach(boton => {
        boton.addEventListener("click", function () {
            let botonId = boton.id;

            fetch("../json/context.json")
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Error en la carga de context.json');
                    }
                    return response.json();
                })
                .then(data => {
                    const objeto = data.find(item => item.id === botonId);

                    if (objeto) {

                        titulo.innerHTML = objeto.title;
                        contenedor.innerHTML = objeto.contenedor;
                        const campos = objeto.campos || [];

                        fetch(objeto.ruta)
                            .then(response2 => {
                                if (!response2.ok) {
                                    throw new Error('Error al cargar los datos desde ' + objeto.ruta);
                                }
                                return response2.json();
                            })
                            .then(data2 => {
                                const cuerpo = document.getElementById("cuerpo-1");
                                if (Array.isArray(data2) && data2.length > 0) {
                                    data2.forEach(item => {
                                        let row = "<tr>";
                                        campos.forEach(campo => {
                                            row += `<td>${item[campo] ?? ""}</td>`;
                                        });
                                        row += `<td><button class="buttonPersonalizado eliminar" id='${objeto.Eliminar}'>Eliminar</button><button class="buttonPersonalizado actulizar" id='${objeto.Actulizar}'>Actualizar</button></td></tr>`;
                                        cuerpo.innerHTML += row;
                                    });
                                } else {
                                    cuerpo.innerHTML = "<tr><td colspan='4'>No hay datos</td></tr>";
                                }
                            })
                            .catch(error => {
                                contenedor.innerHTML += `<p>Error cargando datos: ${error.message}</p>`;
                            });

                        contenedor.innerHTML += `<button onclick="return getBlackList('${objeto.EndPoint}')">Todos los registros</button>`;

                        contenedor.innerHTML += objeto.formulario;

                        const form = document.getElementById("formulario-unico");
                        // llamar el elemento por id-448    48
                        if (form) {
                            form.addEventListener("submit", async function (e) {
                                e.preventDefault();

                                const id = document.getElementById("datoUnico").value;

                                try {
                                    const response = await fetch(`${objeto.EndPoint}/${id}`);
                                    if (!response.ok) {
                                        throw new Error("No se encontró el registro con ese ID");
                                    }

                                    const data = await response.json();
                                    console.log("Datos obtenidos:", data);
                                    // Aquí puedes mostrar los datos si lo deseas

                                } catch (error) {
                                    console.error("Error:", error);
                                    contenedor.innerHTML += `<p style="color:red;">${error.message}</p>`;
                                }
                            });
                        }

                    } else {
                        contenedor.innerHTML = "<p>Contenido no encontrado para el botón.</p>";
                    }

                })
            .catch(error => {
                contenedor.innerHTML = `<p>Error al cargar el contexto: ${error.message}</p>`;
            });
        });
    });
});
// mostrar todas las entidades
async function getBlackList(endpoint) {
    try {
        const response = await fetch(endpoint);
        if (!response.ok) {
            alert("Error al cargar la BlackList");
            throw new Error("No se pudo cargar la BlackList");
        }

        const data = await response.json();
        for (const iteracion in data) {
            if (Object.prototype.hasOwnProperty.call(data, iteracion)) {
                const element = data[iteracion];
                console.log("Datos obtenidos:", element);
            }
        }
    } catch (error) {
        console.error("Error:", error);
    }
}

document.getElementById('blacklistForm').addEventListener('submit', async function (e) {
    e.preventDefault(); // Evita que el formulario recargue la página
    const formData = new FormData(e.target);
    const data = {
      reason: formData.get('reason'),
      restrictiondate: formData.get('restrictiondate'),
      id_client: parseInt(formData.get('id_client'))
    };

    try {
      const response = await fetch('https://localhost:7035/api/BlackList', { // cambia URL según tu API
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });

        const result = await response.json();
        document.getElementById('response').innerText =
        response.ok ? 'Guardado exitosamente' : `Error: ${result.message || 'Error desconocido'}`;
    } catch (error) {
      document.getElementById('response').innerText = 'Error de red: ' + error.message;
    }
  });


// const buttonEliminar = document.querySelector(".eliminar");
// const buttonActulizar = document.querySelector(".actulizar")


// buttonEliminar.addEventListener("click",function(){

// })

// buttonActulizar.addEventListener("click",function(){

// })
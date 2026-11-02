const API_URL = 'http://localhost:5090/api/Pacientes';
let pacientesCache = [];

document.addEventListener('DOMContentLoaded', cargarPacientes);

async function cargarPacientes() {
    try {
        const response = await fetch(API_URL);
        if (response.ok) {
            pacientesCache = await response.json();
            renderizarTabla(pacientesCache);
        }
    } catch (error) {
        console.error("Error al conectar con la API:", error);
    }
}

function renderizarTabla(data) {
    const tbody = document.getElementById('tablaPacientes');
    document.getElementById("contadorPacientes").innerText = `Pacientes registrados: ${data.length}`;
    tbody.innerHTML = '';
    if (data.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" class="px-6 py-8 text-center text-stone-400">No hay pacientes registrados.</td></tr>`;
        return;
    }
    data.forEach(p => {
        const idPaciente = p.id !== undefined ? p.id : p.pacienteId;
        const codigo = p.codigoPaciente || ('PAC-' + String(idPaciente).padStart(4, '0'));
        const fechaReg = p.fechaRegistro ? new Date(p.fechaRegistro).toLocaleDateString() : 'N/D';

        tbody.innerHTML += `
            <tr class="hover:bg-stone-50/50 transition">
                <td class="px-6 py-4 font-bold text-blue-700">${codigo}</td>
                <td class="px-6 py-4 font-mono text-stone-600">${p.cedula}</td>
                <td class="px-6 py-4 font-medium text-stone-900">${p.nombres} ${p.apellidos}</td>
                <td class="px-6 py-4 text-stone-600">${p.telefono}</td>
                <td class="px-6 py-4 text-stone-600">${p.correo || 'N/D'}</td>
                <td class="px-6 py-4 text-stone-600">${fechaReg}</td>
                <td class="px-6 py-4 text-right space-x-3">
                    <button onclick="prepararEdicion(${idPaciente})" class="text-indigo-600 hover:text-indigo-900 font-medium">Editar</button>
                    <button onclick="eliminarPaciente(${idPaciente})" class="text-rose-600 hover:text-rose-800 font-medium">Desactivar</button>
                </td>
            </tr>
        `;
    });
}

function filtrarPacientes() {
    const termino = document.getElementById('busqueda').value.toLowerCase();
    const filtrados = pacientesCache.filter(p =>
        p.cedula.toLowerCase().includes(termino) ||
        p.nombres.toLowerCase().includes(termino) ||
        p.apellidos.toLowerCase().includes(termino)
    );
    renderizarTabla(filtrados);
}

function abrirModal() {
    document.getElementById('modalTitulo').innerText = 'Registrar Nuevo Paciente';
    document.getElementById('formPaciente').reset();
    document.getElementById('pacienteId').value = '';
    bloquearDocumento(false);
    document.getElementById('modalPaciente').classList.remove('hidden');
}

function cerrarModal() {
    document.getElementById('modalPaciente').classList.add('hidden');
    document.getElementById('formPaciente').reset();
}

function bloquearDocumento(bloquear) {
    const campo = document.getElementById('cedula');
    const tipo = document.getElementById('tipoDocumento');
    campo.readOnly = bloquear;
    tipo.disabled = bloquear;
    ['bg-stone-100', 'text-stone-500', 'cursor-not-allowed'].forEach(c => {
        campo.classList.toggle(c, bloquear);
        tipo.classList.toggle(c, bloquear);
    });
    document.getElementById('avisoDocumento').classList.toggle('hidden', !bloquear);
}

function prepararEdicion(id) {
    const paciente = pacientesCache.find(p => (p.id === id || p.pacienteId === id));
    if (!paciente) return;

    document.getElementById('modalTitulo').innerText = 'Editar Paciente';
    document.getElementById('pacienteId').value = paciente.id !== undefined ? paciente.id : paciente.pacienteId;
    document.getElementById('tipoDocumento').value = /^\d{11}$/.test(paciente.cedula) ? 'CEDULA' : 'PASAPORTE';
    document.getElementById('cedula').value = paciente.cedula;
    bloquearDocumento(true);
    document.getElementById('nombres').value = paciente.nombres;
    document.getElementById('apellidos').value = paciente.apellidos;
    if (paciente.fechaNacimiento) {
        document.getElementById('fechaNacimiento').value = paciente.fechaNacimiento.split('T')[0];
    }
    document.getElementById('telefono').value = paciente.telefono;
    document.getElementById('correo').value = paciente.correo || '';
    document.getElementById('direccion').value = paciente.direccion || '';

    document.getElementById('modalPaciente').classList.remove('hidden');
}

async function guardarPaciente(event) {
    event.preventDefault();
    const id = document.getElementById('pacienteId').value;
    const fechaValor = document.getElementById('fechaNacimiento').value;
    const direccion = document.getElementById('direccion').value.trim();

    const tipoDocumento = document.getElementById('tipoDocumento').value;
    const documento = document.getElementById('cedula').value.trim();

    if (tipoDocumento === "CEDULA") {
        if (!/^\d{11}$/.test(documento)) {
            alert("La cédula debe contener exactamente 11 números.");
            return;
        }
    }

    if (tipoDocumento === "PASAPORTE") {
        if (documento.length < 5) {
            alert("Ingrese un número de pasaporte válido.");
            return;
        }
    }

    if (!fechaValor) {
        alert("La fecha de nacimiento es obligatoria.");
        return;
    }

    if (fechaValor && new Date(fechaValor) > new Date()) {
        alert("La fecha de nacimiento no puede ser futura.");
        return;
    }

    if (direccion === "") {
        alert("La dirección es obligatoria.");
        return;
    }

    const paciente = {
        cedula: documento,
        nombres: document.getElementById('nombres').value,
        apellidos: document.getElementById('apellidos').value,
        fechaNacimiento: fechaValor,
        telefono: document.getElementById('telefono').value,
        correo: document.getElementById('correo').value || null,
        direccion: direccion
    };

    const method = id ? 'PUT' : 'POST';
    const url = id ? `${API_URL}/${id}` : API_URL;

    try {
        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(paciente)
        });

        if (response.ok) {
            alert(id ? "Paciente actualizado correctamente." : "Paciente registrado correctamente.");
            cerrarModal();
            cargarPacientes();
        } else {
            const errorData = await response.json().catch(() => ({}));
            alert(errorData.mensaje || "Error al guardar el paciente.");
        }
    } catch (error) {
        console.error("Error de red:", error);
        alert("Error de conexión con el servidor.");
    }
}

async function eliminarPaciente(id) {
    if (!confirm("¿Deseas desactivar este paciente?")) return;
    try {
        const response = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
        if (response.ok) {
            cargarPacientes();
        }
    } catch (error) {
        console.error("Error al eliminar:", error);
    }
}

document.getElementById("tipoDocumento").addEventListener("change", function () {
    const campo = document.getElementById("cedula");
    if (this.value === "CEDULA") {
        campo.placeholder = "Ej. 40212345678";
    } else {
        campo.placeholder = "Ej. A1234567";
    }
});
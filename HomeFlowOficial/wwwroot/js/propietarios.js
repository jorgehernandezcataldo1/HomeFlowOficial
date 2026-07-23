document.addEventListener("DOMContentLoaded", function () {
    const formulario = document.getElementById("formCrearPropietario");
    if (!formulario) return;

    formulario.addEventListener("submit", async function (evento) {
        evento.preventDefault();
        limpiarErrores();

        const datos = new FormData(formulario);
        const token = formulario.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            const respuesta = await fetch("/Propietarios/Crear", {
                method: "POST",
                headers: { "RequestVerificationToken": token },
                body: datos
            });

            const resultado = await respuesta.json();

            if (respuesta.ok && resultado.exito) {
                mostrarAlerta("success", resultado.mensaje);
                // Cierra el modal simulando el click en el botón declarativo
                // (evita instanciar bootstrap.Modal() por JS).
                document.getElementById("btnCerrarModalCrear").click();
                setTimeout(() => window.location.reload(), 700);
            } else {
                mostrarErrores(resultado.errores);
            }
        } catch (error) {
            mostrarAlerta("danger", "Ocurrió un error inesperado. Intenta nuevamente.");
        }
    });

    function limpiarErrores() {
        formulario.querySelectorAll(".is-invalid").forEach(el => el.classList.remove("is-invalid"));
        formulario.querySelectorAll(".invalid-feedback").forEach(el => el.textContent = "");
    }

    function mostrarErrores(errores) {
        if (!errores) return;
        Object.keys(errores).forEach(campo => {
            const input = formulario.querySelector(`[name="${campo}"]`);
            const feedback = formulario.querySelector(`[data-field="${campo}"]`);
            if (input) input.classList.add("is-invalid");
            if (feedback) feedback.textContent = errores[campo][0];
        });
    }

    function mostrarAlerta(tipo, mensaje) {
        const contenedor = document.getElementById("alertaContenedor");
        if (!contenedor) return;
        contenedor.innerHTML = `
            <div class="alert alert-${tipo} alert-dismissible fade show" role="alert">
                ${mensaje}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>`;
    }
});

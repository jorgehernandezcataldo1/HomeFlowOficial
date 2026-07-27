$(function () {
    const $formulario = $('#formCrearInmueble');
    if ($formulario.length === 0) return;

    $formulario.on('submit', function (evento) {
        evento.preventDefault();
        limpiarErrores();

        const datos = new FormData(this);
        const token = $formulario.find('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: '/Inmueble/Crear',
            type: 'POST',
            data: datos,
            processData: false,   // evita que $.ajax intente serializar el FormData
            contentType: false,   // deja que el navegador arme el multipart/form-data
            headers: { 'RequestVerificationToken': token },
            dataType: 'json',
            success: function (resultado) {
                mostrarAlerta('success', resultado.mensaje);
                document.getElementById('btnCerrarModalCrearInmueble').click();
                setTimeout(function () { window.location.reload(); }, 700);
            },
            error: function (xhr) {
                if (xhr.status === 400 && xhr.responseJSON) {
                    mostrarErrores(xhr.responseJSON.errores);
                } else {
                    mostrarAlerta('danger', 'Ocurrió un error inesperado. Intenta nuevamente.');
                }
            }
        });
    });

    function limpiarErrores() {
        $formulario.find('.is-invalid').removeClass('is-invalid');
        $formulario.find('.invalid-feedback').text('');
    }

    function mostrarErrores(errores) {
        if (!errores) return;
        Object.keys(errores).forEach(function (campo) {
            const $input = $formulario.find('[name="' + campo + '"]');
            const $feedback = $formulario.find('[data-field="' + campo + '"]');
            $input.addClass('is-invalid');
            $feedback.text(errores[campo][0]);
        });
    }

    function mostrarAlerta(tipo, mensaje) {
        const $contenedor = $('#alertaContenedor');
        if ($contenedor.length === 0) return;
        $contenedor.html(
            '<div class="alert alert-' + tipo + ' alert-dismissible fade show" role="alert">' +
            mensaje +
            '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>' +
            '</div>'
        );
    }
});

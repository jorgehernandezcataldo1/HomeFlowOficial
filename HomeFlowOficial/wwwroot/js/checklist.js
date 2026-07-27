$(function () {
    const $modal = $('#modalChecklist');
    if ($modal.length === 0) return;

    const $form = $('#formChecklist');
    const $contenedor = $('#checklistItemsContenedor');
    const $sinDatos = $('#checklistSinDatos');
    const tokenHeaderNombre = 'X-CSRF-TOKEN';

    function obtenerToken() {
        return $form.find('input[name="__RequestVerificationToken"]').val();
    }

    // Bootstrap dispara este evento nativo cuando el modal se abre por el atributo
    // data-bs-toggle/data-bs-target del botón que lo gatilla; solo escuchamos, no
    // instanciamos el modal por JS en ningún momento.
    $modal.on('show.bs.modal', function (evento) {
        const $boton = $(evento.relatedTarget);
        const tipoEntidad = $boton.data('tipo-entidad');
        const entidadId = $boton.data('entidad-id');
        const nombre = $boton.data('nombre');

        $('#modalChecklistLabel').text('Checklist — ' + (nombre || ''));
        $('#checklistTipoEntidad').val(tipoEntidad);
        $('#checklistEntidadId').val(entidadId);
        $contenedor.empty();
        $sinDatos.addClass('d-none');

        $.ajax({
            url: '/Checklist/Obtener',
            type: 'GET',
            data: { tipoEntidad: tipoEntidad, entidadId: entidadId },
            dataType: 'json',
            success: function (datos) {
                $('#checklistPlantillaId').val(datos.checklistPlantillaId);
                $('#checklistNombrePlantilla').text(datos.nombre);
                pintarItems(datos.items);
            },
            error: function () {
                $sinDatos.removeClass('d-none');
                $('#checklistNombrePlantilla').text('');
            }
        });
    });

    function pintarItems(items) {
        $contenedor.empty();
        items.forEach(function (item) {
            const marcado = item.cumple ? 'checked' : '';
            const obligatorio = item.obligatorio ? ' <span class="text-danger">*</span>' : '';
            const observacion = item.observacion ? item.observacion.replace(/"/g, '&quot;') : '';

            const html =
                '<div class="form-check border-bottom py-2">' +
                '  <input class="form-check-input" type="checkbox" data-item-id="' + item.id + '" id="chkItem' + item.id + '" ' + marcado + '>' +
                '  <label class="form-check-label" for="chkItem' + item.id + '">' + item.descripcion + obligatorio + '</label>' +
                '  <input type="text" class="form-control form-control-sm mt-1" placeholder="Observación (opcional)" data-obs-id="' + item.id + '" value="' + observacion + '">' +
                '</div>';

            $contenedor.append(html);
        });
    }

    $form.on('submit', function (evento) {
        evento.preventDefault();

        const items = [];
        $contenedor.find('[data-item-id]').each(function () {
            const id = $(this).data('item-id');
            items.push({
                ChecklistItemId: id,
                Cumple: $(this).is(':checked'),
                Observacion: $('[data-obs-id="' + id + '"]').val() || null
            });
        });

        if (items.length === 0) return;

        const payload = {
            ChecklistPlantillaId: $('#checklistPlantillaId').val(),
            TipoEntidad: $('#checklistTipoEntidad').val(),
            EntidadId: $('#checklistEntidadId').val(),
            Items: items
        };

        const encabezados = {};
        encabezados[tokenHeaderNombre] = obtenerToken();

        $.ajax({
            url: '/Checklist/Guardar',
            type: 'POST',
            contentType: 'application/json',
            headers: encabezados,
            data: JSON.stringify(payload),
            dataType: 'json',
            success: function () {
                document.getElementById('btnCerrarModalChecklist').click();
                setTimeout(function () { window.location.reload(); }, 700);
            },
            error: function (xhr) {
                const mensaje = (xhr.responseJSON && xhr.responseJSON.mensaje) || 'No se pudo guardar el checklist.';
                alert(mensaje);
            }
        });
    });
});

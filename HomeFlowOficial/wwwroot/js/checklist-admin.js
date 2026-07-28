(function () {
    function filaItem(descripcion = '', obligatorio = true) {
        return `
            <div class="row g-2 align-items-center mb-2 fila-item-checklist">
                <div class="col-8">
                    <input type="text" class="form-control form-control-sm item-descripcion"
                           placeholder="Descripción del ítem" value="${$('<div>').text(descripcion).html()}" required />
                </div>
                <div class="col-3 form-check">
                    <input type="checkbox" class="form-check-input item-obligatorio" ${obligatorio ? 'checked' : ''} />
                    <label class="form-check-label small">Obligatorio</label>
                </div>
                <div class="col-1">
                    <button type="button" class="btn btn-sm btn-outline-danger btn-quitar-item">&times;</button>
                </div>
            </div>`;
    }

    function limpiarModal() {
        $('#checklistAdminPlantillaBaseId, #checklistAdminTipoEntidad, #checklistAdminNombre').val('');
        $('#checklistAdminItemsContenedor').empty();
    }

    function mostrarAlerta(tipo, mensaje) {
        $('#alertaContenedor').html(
            `<div class="alert alert-${tipo} alert-dismissible fade show">${mensaje}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button></div>`);
    }

    $(document).on('click', '.btn-nuevo-checklist', function () {
        limpiarModal();
        $('#checklistAdminTipoEntidad').val($(this).data('tipo-entidad'));
        $('#modalChecklistAdminLabel').text('Nuevo checklist — ' + $(this).data('nombre-tipo'));
        $('#checklistAdminItemsContenedor').append(filaItem());
        new bootstrap.Modal('#modalChecklistAdmin').show();
    });

    $(document).on('click', '.btn-editar-checklist', function () {
        limpiarModal();
        $.get('/ChecklistAdmin/Obtener', { id: $(this).data('id') }).done(function (data) {
            $('#checklistAdminPlantillaBaseId').val(data.id);
            $('#checklistAdminTipoEntidad').val(data.tipoEntidad);
            $('#checklistAdminNombre').val(data.nombre);
            $('#modalChecklistAdminLabel').text('Editar checklist (creará v' + (data.version + 1) + ')');
            const c = $('#checklistAdminItemsContenedor').empty();
            data.items.forEach(i => c.append(filaItem(i.descripcion, i.obligatorio)));
            new bootstrap.Modal('#modalChecklistAdmin').show();
        }).fail(() => mostrarAlerta('danger', 'No se pudo cargar el checklist.'));
    });

    $(document).on('click', '.btn-archivar-checklist', function () {
        if (!confirm('¿Archivar este checklist?')) return;
        $.ajax({
            url: '/ChecklistAdmin/Archivar', method: 'POST', data: { id: $(this).data('id') },
            headers: { 'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val() }
        }).done(() => location.reload())
            .fail(() => mostrarAlerta('danger', 'No se pudo archivar.'));
    });

    $(document).on('click', '#btnAgregarItemChecklist', () => $('#checklistAdminItemsContenedor').append(filaItem()));
    $(document).on('click', '.btn-quitar-item', function () { $(this).closest('.fila-item-checklist').remove(); });

    $('#formChecklistAdmin').on('submit', function (e) {
        e.preventDefault();

        const items = [];
        $('.fila-item-checklist').each(function (indice) {
            items.push({
                descripcion: $(this).find('.item-descripcion').val(),
                obligatorio: $(this).find('.item-obligatorio').is(':checked'),
                orden: indice + 1
            });
        });

        const baseIdRaw = $('#checklistAdminPlantillaBaseId').val();
        const payload = {
            plantillaBaseId: baseIdRaw ? parseInt(baseIdRaw) : null,
            nombre: $('#checklistAdminNombre').val(),
            tipoEntidad: parseInt($('#checklistAdminTipoEntidad').val()),
            items: items
        };

        $.ajax({
            url: '/ChecklistAdmin/Guardar', method: 'POST', contentType: 'application/json',
            data: JSON.stringify(payload),
            headers: { 'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val() }
        }).done(() => {
            bootstrap.Modal.getInstance(document.getElementById('modalChecklistAdmin')).hide();
            location.reload();
        }).fail(xhr => mostrarAlerta('danger', xhr.responseJSON?.mensaje || 'No se pudo guardar.'));
    });
})();
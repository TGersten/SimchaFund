$(() => {

    console.log("the contributor page")
    const contributorModal = new bootstrap.Modal($(".new-contrib")[0]);

    $("#new-contributor").on("click", function () {
        console.log("click")

        contributorModal.show();



    })

    const depositModal = new bootstrap.Modal($(".deposit")[0]);

    $(".deposit-button").on("click", function () {
        console.log("click")

        const id = $(this).data('id');
        console.log(id);
        console.log("past contributor id")
        const firstName = $(this).data('first-name')
        const lastName = $(this).data('last-name')
        const fullName = firstName + lastName;

   
        $('#contributor-id').val(id);

        $("#deposit-name").text(fullName);


        depositModal.show();



    })

    $(".edit-contrib").on("click", function () {
        console.log("click")

        const editModal = contributorModal;

        const contributorId = $(this).data('id');
        const firstName = $(this).data('first-name');
        const lastName = $(this).data('last-name');
        const cellNumber = $(this).data('cell');
        const date = $(this).data('date');
        const alwaysInclude = $(this).data('always-include');

        $("#contributor_first_name").val(firstName);
        $("#contributor_last_name").val(lastName);
        $("#contributor_cell_number").val(cellNumber);
        $("#contributor_created_at").val(date);      
        $("#contributor_always_include").prop('checked', alwaysInclude === "True");
        $("#initialDepositDiv").hide();
        $("#edit-contributor-form").attr('action', "/contributors/edit");
        $("#edit-contributor-form").append(`<input type="hidden" id="contributor-id"name="Id" value="${contributorId}" />`)
        $("#modal-title").text('Edit Contributer')

        editModal.show();




    })
})


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

        const contributorId = $(this).data('id');
        console.log(contributorId);
        $('#contributor-id').val(contributorId);


        depositModal.show();



    })

    $(".edit-contrib").on("click", function () {
        console.log("click")

        const editModal = contributorModal;

        const contributorId = $(this).data('id');
        const firstName = $(this).data('first-name');
        $("#contributor_first_name").val(firstName);

        const lastName = $(this).data('last-name');
        $("#contributor_last_name").val(lastName);

        const cellNumber = $(this).data('cell');
        $("#contributor_cell_number").val(cellNumber);

        const date = $(this).data('date');     
        $("#contributor_created_at").val(date);

        const alwaysInclude = $(this).data('always-include');
        console.log(alwaysInclude);
        
        $("#contributor_always_include").prop('checked', !alwaysInclude );

        $("#initialDepositDiv").hide();



        editModal.show();




    })
})


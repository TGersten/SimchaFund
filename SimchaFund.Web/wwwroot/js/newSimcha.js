$(() => {

    console.log("the simcha page")
    const modal = new bootstrap.Modal($("#new-simcha-modal")[0]);

    $("#new-simcha").on("click", function () {
        console.log("click")
        
        modal.show();



    })



})
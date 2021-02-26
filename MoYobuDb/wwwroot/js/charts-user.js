window.onload = function () {
    var userName = $("#h_var1").val();
    // var basicStatsAnimePath = 'https://172.21.0.13:5001/API/GET/CHARTS/USER/' + userName + '/ANIME';
    // var basicStatsAnimePath = 'https://90.176.72.74:5001/API/GET/CHARTS/USER/' + userName + '/ANIME';
    var basicStatsAnimePath = 'https://localhost:5001/API/GET/CHARTS/USER/' + userName + '/ANIME';
    // var basicStatsMangaPath = 'https://172.21.0.13:5001/API/GET/CHARTS/USER/' + userName + '/MANGA';
    // var basicStatsMangaPath = 'https://90.176.72.74:5001/API/GET/CHARTS/USER/' + userName + '/MANGA';
    var basicStatsMangaPath = 'https://localhost:5001/API/GET/CHARTS/USER/' + userName + '/MANGA';
    
    $.getJSON(basicStatsAnimePath,function(data){
        var ctx = document.getElementById("myChart").getContext("2d");
        var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: "doughnut",

            // The data for our dataset
            data: {
                labels: ["tv", "ova", "movie", "music", "special", "ona", "tv short"],
                datasets: [
                    {
                        // label: 'Format distribution',
                        backgroundColor: [
                            "#b00038",
                            "#848403",
                            "#006384",
                            "#84004d",
                            "#5f0084",
                            "#008433",
                            "#844b00"
                        ],
                        hoverBackgroundColor: [
                            "#760026",
                            "#6c6c02",
                            "#004d84",
                            "#6a003e",
                            "#490064",
                            "#006d2a",
                            "#713d00"
                        ],
                        hoverBorderWidth: 3,

                        borderColor: "rgba(0, 0, 0, 0)",
                        data: [data.tv, data.ova, data.movie, data.music, data.special, data.ona, data.tvShort],
                    },
                ],
            },

            // Configuration options go here
            options: {
                legend: {
                    display: true,
                    position: "right",
                    align: "center",
                    labels: {
                        fontColor: "#d9d9d9",
                    },
                },

                title: {
                    display: true,
                    text: "Nejsledovanější formát",
                    fontSize: 20,
                    padding: 20,
                    lineHeight: 3,
                    position: "top",
                    fontColor: "#d9d9d9",
                },
                responsive: true,
            },
        });
    });

    $.getJSON(basicStatsMangaPath,function(data){
        var ctx = document.getElementById("myChart2").getContext("2d");
        var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: "doughnut",

            // The data for our dataset
            data: {
                labels: ["novel", "manga", "one shot"],
                datasets: [
                    {
                        // label: 'Format distribution',
                        backgroundColor: [
                            "#950031",
                            "#008433",
                            "#006384"
                        ],
                        hoverBackgroundColor: [
                            "#760026",
                            "#006d2a",
                            "#004d84"
                        ],
                        hoverBorderWidth: 3,

                        borderColor: "rgba(0, 0, 0, 0)",
                        data: [data.novel, data.manga, data.oneShot],
                    },
                ],
            },

            // Configuration options go here
            options: {
                legend: {
                    display: true,
                    position: "right",
                    align: "center",
                    labels: {
                        fontColor: "#d9d9d9",
                    },
                },

                title: {
                    display: true,
                    text: "Nejčtenější formát",
                    fontSize: 20,
                    padding: 20,
                    lineHeight: 3,
                    position: "top",
                    fontColor: "#d9d9d9",
                },
                responsive: true,
            },
        });
    });
};
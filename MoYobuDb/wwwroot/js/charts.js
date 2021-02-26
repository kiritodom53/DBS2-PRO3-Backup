window.onload = function () {

    // var mediaChartsPath = 'https://172.21.0.13:5001/API/GET/CHARTS/MEDIA';
    // var topGenresPath = 'https://172.21.0.13:5001/API/GET/CHARTS/TOPGENRES';
    // var animeByYears = 'https://172.21.0.13:5001/API/GET/CHARTS/ANIMEBYYEARS';
    var mediaChartsPath = 'https://localhost:5001/API/GET/CHARTS/MEDIA';
    var topGenresPath = 'https://localhost:5001/API/GET/CHARTS/TOPGENRES';
    var animeByYears = 'https://localhost:5001/API/GET/CHARTS/ANIMEBYYEARS';
    // var mediaChartsPath = 'https://90.176.72.74:5001/API/GET/CHARTS/MEDIA';
    // var topGenresPath = 'https://90.176.72.74:5001/API/GET/CHARTS/TOPGENRES';
    // var animeByYears = 'https://90.176.72.74:5001/API/GET/CHARTS/ANIMEBYYEARS';
    


    $.getJSON(mediaChartsPath,function(dataMedia){
        var ctx = document.getElementById("myChart").getContext("2d");
        var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: "doughnut",

            // The data for our dataset
            data: {
                labels: ["Anime", "Manga"],
                datasets: [
                    {
                        // label: 'Format distribution',
                        backgroundColor: ["#b00038", "rgb(0, 99, 132)"],
                        hoverBackgroundColor: [
                            "#760026",
                            "rgb(0, 77, 132)",
                        ],
                        hoverBorderWidth: 3,

                        borderColor: "rgba(0, 0, 0, 0)",
                        data: [dataMedia.anime, dataMedia.manga],
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
                    text: "Anime / Manga",
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

    $.getJSON(topGenresPath,function(topGenres){
        var ctx2 = document.getElementById("myChart2").getContext("2d");
        var myRadarChart = new Chart(ctx2, {
            type: "bar",
            title: "Top žánry",

            data: {
                labels: [
                    topGenres[0].title,
                    topGenres[1].title,
                    topGenres[2].title,
                    topGenres[3].title,
                    topGenres[4].title,
                    topGenres[5].title,
                ],
                datasets: [
                    {
                        backgroundColor: [
                            "#b00038",
                            "#006384",
                            "#00844a",
                            "#530084",
                            "#848200",
                            "#840145",
                        ],
                        hoverBackgroundColor: [
                            "#820029",
                            "#00506b",
                            "#00673a",
                            "#3d0061",
                            "#666400",
                            "#620133"
                        ],
                        hoverBorderWidth: 3,

                        borderColor: "rgba(0, 0, 0, 0)",
                        data: [topGenres[0].count, topGenres[1].count, topGenres[2].count, topGenres[3].count, topGenres[4].count, topGenres[5].count],
                    },
                ],
            },
            options: {
                scales: {
                    arc: [
                        {
                            borderColor: "#d9d9d963",
                        },
                    ],
                },
                startAngle: 0.75 * Math.PI,
                legend: {
                    display: false,
                    position: "right",
                    align: "center",
                    labels: {
                        fontColor: "#d9d9d9",
                    },
                },
                title: {
                    display: true,
                    text: "Top žánry",
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
    
    //animeByYears
    $.getJSON(animeByYears,function(animeYears){
        var ctx3 = document.getElementById("myChart3").getContext("2d");
        var myRadarChart = new Chart(ctx3, {
            type: "line",
            title: "released this year",

            data: {
                labels: [
                    animeYears[0].year,
                    animeYears[2].year,
                    animeYears[3].year,
                    animeYears[4].year,
                    animeYears[5].year,
                    animeYears[6].year,
                    animeYears[7].year,
                    animeYears[8].year,
                    animeYears[9].year,
                    animeYears[10].year,
                    animeYears[11].year,
                    animeYears[12].year,
                    animeYears[13].year,
                    animeYears[14].year,
                    animeYears[15].year,
                    animeYears[16].year,
                    animeYears[17].year,
                    animeYears[18].year,
                    animeYears[19].year,
                    animeYears[20].year,
                    animeYears[21].year,
                    animeYears[22].year,
                    animeYears[23].year,
                    animeYears[24].year,
                    animeYears[25].year,
                    animeYears[26].year,
                    animeYears[27].year,
                    animeYears[28].year,
                    animeYears[29].year,
                    animeYears[30].year
                ],
                datasets: [
                    {
                        label: "Released this year",
                        backgroundColor: "#790026",
                        hoverBackgroundColor: "rgb(255, 99, 110)",

                        hoverBorderWidth: 10,

                        pointBackground: "rgb(0,0,0)",
                        pointHoverBackgroundColor: "#790026",

                        borderColor: "rgb(0, 0, 0)",

                        data: [
                            animeYears[0].counter,
                            animeYears[2].counter,
                            animeYears[3].counter,
                            animeYears[4].counter,
                            animeYears[5].counter,
                            animeYears[6].counter,
                            animeYears[7].counter,
                            animeYears[8].counter,
                            animeYears[9].counter,
                            animeYears[10].counter,
                            animeYears[11].counter,
                            animeYears[12].counter,
                            animeYears[13].counter,
                            animeYears[14].counter,
                            animeYears[15].counter,
                            animeYears[16].counter,
                            animeYears[17].counter,
                            animeYears[18].counter,
                            animeYears[19].counter,
                            animeYears[20].counter,
                            animeYears[21].counter,
                            animeYears[22].counter,
                            animeYears[23].counter,
                            animeYears[24].counter,
                            animeYears[25].counter,
                            animeYears[26].counter,
                            animeYears[27].counter,
                            animeYears[28].counter,
                            animeYears[29].counter,
                            animeYears[30].counter
                        ],
                    },
                ],
            },
            options: {
                scales: {
                    xAxes: [
                        {
                            gridLines: {
                                color: "#d9d9d963",
                            },
                            ticks: {
                                fontColor: "#d9d9d9",
                            },
                        },
                    ],
                    yAxes: [
                        {
                            gridLines: {
                                color: "#d9d9d963",
                            },
                            ticks: {
                                fontColor: "#d9d9d9",
                            },
                        },
                    ],
                },
                legend: {
                    display: false,
                    position: "right",
                    align: "center",
                },
                title: {
                    display: true,
                    text: "Tituly za rok",
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

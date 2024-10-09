function renderChart(elementId, chartData) {
    var ctx = document.getElementById(elementId).getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: chartData,
        options: {
            maintainAspectRatio: false,
            responsive: true,
            plugins: {
                legend: {
                    display: false,
                }
            },
            animations: {
                tension: {
                    duration: 500,
                    easing: 'easeInOutBounce',
                    from: 0.4,
                    to: 0.3,
                    loop: true
                }
            },
            scales: {
                x: {
                    display: false
                },
                y: {
                    beginAtZero: true
                }
            }
        }

    });
}


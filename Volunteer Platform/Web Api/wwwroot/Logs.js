function showLogs() {
    
    const n = $('#log-count').val();
    const token = localStorage.getItem('JWT');
    console.log(token);

    if (!token) {
        alert('No token found, please login.');
        window.location.href = 'login.html';
        return;
    }

    $.ajax({
        url: `http://localhost:5158/api/log/get/${n}`,
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        success: function (logs) {
            $('#log-list').empty();
            logs.forEach(log => {
                let timestamp = new Date(log.timestamp);
                let formattedTimestamp = `${timestamp.getDate().toString().padStart(2, '0')}.${(timestamp.getMonth() + 1).toString().padStart(2, '0')}.${timestamp.getFullYear().toString().slice(-2)} ${timestamp.getHours().toString().padStart(2, '0')}:${timestamp.getMinutes().toString().padStart(2, '0')}`;
                $('#log-list').append(`<li>${formattedTimestamp} || ${log.level} || ${log.message}</li>`);
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error loading logs:', textStatus, errorThrown);
            alert('Failed to load logs. Please check your authentication.');
        }
    });
}

function logout() {
    localStorage.removeItem('JWT');
    window.location.href = 'login.html';
}


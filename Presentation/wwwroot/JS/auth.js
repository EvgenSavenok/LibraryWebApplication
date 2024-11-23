function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

async function checkAndRefreshToken() {
    const token = localStorage.getItem('authToken');
    if (!token) {
        window.location.href = 'api/authentication/loginPage';
        return;
    }
    const accessTokenLifetime = 5000;
    const decodedToken = parseJwt(token);
    const expirationTime = decodedToken.exp * 1000;
    const currentTime = Date.now();
    if (expirationTime - currentTime < accessTokenLifetime) {
        const refreshToken = localStorage.getItem('refreshToken');
        const accessToken = localStorage.getItem('authToken');
        const response = await fetch('/api/token/refresh', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ accessToken, refreshToken })
        });

        if (response.ok) {
            const result = await response.json();
            localStorage.setItem('authToken', result.accessToken); 
            localStorage.setItem('refreshToken', result.refreshToken); 
        } else {
            window.location.href = '/api/authentication/loginPage';
        }
    }
}
document.addEventListener('DOMContentLoaded', function () {
    const chatFab = document.getElementById('chat-fab');
    const chatWidget = document.getElementById('chat-widget');
    const closeWidgetBtn = document.getElementById('close-chat-widget');
    const widgetSendBtn = document.getElementById('chat-widget-send');
    const widgetInput = document.getElementById('chat-widget-input');
    const widgetMessages = document.getElementById('chat-widget-messages');

    let isChatLoaded = false;

    // Abrir widget
    if (chatFab) {
        chatFab.addEventListener('click', function () {
            chatWidget.classList.add('active');
            chatFab.style.display = 'none'; // Ocultar botón flotante al abrir
            if (!isChatLoaded) {
                loadChatHistory();
                isChatLoaded = true;
            }
            // Scroll al fondo
            setTimeout(() => {
                widgetMessages.scrollTop = widgetMessages.scrollHeight;
            }, 100);
        });
    }

    // Cerrar widget
    if (closeWidgetBtn) {
        closeWidgetBtn.addEventListener('click', function () {
            chatWidget.classList.remove('active');
            chatFab.style.display = 'flex'; // Mostrar botón flotante
        });
    }

    // Enviar mensaje con click
    if (widgetSendBtn) {
        widgetSendBtn.addEventListener('click', sendMessageWidget);
    }

    // Enviar mensaje con Enter
    if (widgetInput) {
        widgetInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                sendMessageWidget();
            }
        });
    }

    async function sendMessageWidget() {
        const message = widgetInput.value.trim();
        if (!message) return;

        // Mostrar mensaje usuario
        appendMessage(message, true);
        widgetInput.value = '';
        
        // Indicador de carga (opcional)
        const loadingDiv = document.createElement('div');
        loadingDiv.className = 'message bot loading';
        loadingDiv.textContent = 'Escribiendo...';
        widgetMessages.appendChild(loadingDiv);
        widgetMessages.scrollTop = widgetMessages.scrollHeight;

        try {
            const response = await fetch('/Chat/SendMessage', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Message: message })
            });
            const data = await response.json();
            
            // Remover loading
            widgetMessages.removeChild(loadingDiv);

            if (data.success) {
                appendMessage(data.message, false);
            } else {
                appendMessage("Error: " + data.message, false);
            }
        } catch (error) {
            if(widgetMessages.contains(loadingDiv)) widgetMessages.removeChild(loadingDiv);
            appendMessage("Error de conexión", false);
        }
    }

    async function loadChatHistory() {
        try {
            const response = await fetch('/Chat/GetHistory');
            const data = await response.json();
            
            if (data.success && data.history) {
                widgetMessages.innerHTML = ''; // Limpiar
                data.history.forEach(msg => {
                    // msg.isUser y msg.content vienen del modelo C#
                    appendMessage(msg.content, msg.isUser);
                });
            }
        } catch (error) {
            console.error("Error cargando historial", error);
        }
    }

    function appendMessage(text, isUser) {
        const div = document.createElement('div');
        div.className = isUser ? 'message user' : 'message bot';
        // Reemplazar saltos de línea por <br>
        div.innerHTML = text.replace(/\n/g, '<br>'); 
        widgetMessages.appendChild(div);
        widgetMessages.scrollTop = widgetMessages.scrollHeight;
    }
});

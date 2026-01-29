// PayPal Integration for Citadel Worship Ministries
let paypalScriptLoaded = false;
let currentPaypalClientId = null;

window.loadPayPalButtons = async function (clientId, amount, giftType, dotNetRef) {
    const container = document.getElementById('paypal-button-container');
    if (!container) return;

    // Clear existing buttons
    container.innerHTML = '';

    // Load PayPal script if not already loaded or if client ID changed
    if (!paypalScriptLoaded || currentPaypalClientId !== clientId) {
        // Remove existing script if any
        const existingScript = document.getElementById('paypal-sdk');
        if (existingScript) {
            existingScript.remove();
        }

        await new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.id = 'paypal-sdk';
            script.src = `https://www.paypal.com/sdk/js?client-id=${clientId}&currency=USD`;
            script.onload = () => {
                paypalScriptLoaded = true;
                currentPaypalClientId = clientId;
                resolve();
            };
            script.onerror = reject;
            document.head.appendChild(script);
        });
    }

    // Render PayPal buttons
    paypal.Buttons({
        style: {
            layout: 'vertical',
            color: 'blue',
            shape: 'rect',
            label: 'paypal'
        },

        createOrder: function (data, actions) {
            return actions.order.create({
                purchase_units: [{
                    description: `${giftType} - Citadel Worship Ministries`,
                    amount: {
                        value: amount.toFixed(2),
                        currency_code: 'USD'
                    }
                }]
            });
        },

        onApprove: async function (data, actions) {
            const order = await actions.order.capture();
            const transactionId = order.purchase_units[0].payments.captures[0].id;
            const paidAmount = parseFloat(order.purchase_units[0].payments.captures[0].amount.value);

            // Call back to Blazor
            await dotNetRef.invokeMethodAsync('OnPaymentSuccess', transactionId, paidAmount);
        },

        onError: async function (err) {
            console.error('PayPal Error:', err);
            await dotNetRef.invokeMethodAsync('OnPaymentError', err.message || 'Payment failed');
        },

        onCancel: function (data) {
            console.log('Payment cancelled');
        }

    }).render('#paypal-button-container');
};

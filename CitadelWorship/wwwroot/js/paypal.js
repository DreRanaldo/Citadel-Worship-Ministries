// PayPal Integration for Citadel Worship Ministries
let paypalScriptLoaded = false;
let currentPaypalClientId = null;
let currentButtonInstance = null;

window.loadPayPalButtons = async function (clientId, amount, giftType, dotNetRef) {
    const container = document.getElementById('paypal-button-container');
    if (!container) {
        console.error('PayPal: Container #paypal-button-container not found');
        return;
    }

    // Clear existing buttons
    container.innerHTML = '';
    currentButtonInstance = null;

    // Load PayPal SDK script if not loaded or client ID changed
    if (!paypalScriptLoaded || currentPaypalClientId !== clientId) {
        const existingScript = document.getElementById('paypal-sdk');
        if (existingScript) {
            existingScript.remove();
            paypalScriptLoaded = false;
        }

        try {
            await new Promise((resolve, reject) => {
                const script = document.createElement('script');
                script.id = 'paypal-sdk';
                script.src = 'https://www.paypal.com/sdk/js?client-id=' + clientId + '&currency=USD&intent=capture';
                script.onload = () => {
                    paypalScriptLoaded = true;
                    currentPaypalClientId = clientId;
                    console.log('PayPal SDK loaded successfully');
                    resolve();
                };
                script.onerror = (e) => {
                    console.error('PayPal SDK failed to load', e);
                    reject(new Error('Failed to load PayPal SDK. Check your Client ID.'));
                };
                document.head.appendChild(script);
            });
        } catch (err) {
            container.innerHTML = '<div class="alert alert-danger"><i class="bi bi-exclamation-triangle"></i> Failed to load PayPal. Please check your internet connection and PayPal Client ID.</div>';
            return;
        }
    }

    // Ensure PayPal is available
    if (typeof paypal === 'undefined') {
        container.innerHTML = '<div class="alert alert-danger"><i class="bi bi-exclamation-triangle"></i> PayPal SDK not available. Please refresh the page.</div>';
        return;
    }

    try {
        currentButtonInstance = paypal.Buttons({
            style: {
                layout: 'vertical',
                color: 'gold',
                shape: 'rect',
                label: 'paypal',
                height: 45
            },

            createOrder: function (data, actions) {
                console.log('PayPal: Creating order for $' + amount.toFixed(2) + ' (' + giftType + ')');
                return actions.order.create({
                    purchase_units: [{
                        description: giftType + ' - Citadel Worship Ministries',
                        amount: {
                            value: amount.toFixed(2),
                            currency_code: 'USD'
                        }
                    }],
                    application_context: {
                        brand_name: 'Citadel Worship Ministries',
                        shipping_preference: 'NO_SHIPPING'
                    }
                });
            },

            onApprove: async function (data, actions) {
                console.log('PayPal: Payment approved, capturing order...');
                try {
                    const order = await actions.order.capture();
                    console.log('PayPal: Order captured', order);

                    const capture = order.purchase_units[0].payments.captures[0];
                    const transactionId = capture.id;
                    const paidAmount = parseFloat(capture.amount.value);
                    const payerName = order.payer ? (order.payer.name.given_name + ' ' + order.payer.name.surname) : 'N/A';
                    const payerEmail = order.payer ? order.payer.email_address : 'N/A';

                    console.log('PayPal: Transaction ID: ' + transactionId + ', Amount: $' + paidAmount);
                    console.log('PayPal: Payer: ' + payerName + ' (' + payerEmail + ')');

                    await dotNetRef.invokeMethodAsync('OnPaymentSuccess', transactionId, paidAmount, payerName, payerEmail);
                } catch (captureErr) {
                    console.error('PayPal: Capture failed', captureErr);
                    await dotNetRef.invokeMethodAsync('OnPaymentError', 'Payment capture failed: ' + (captureErr.message || 'Unknown error'));
                }
            },

            onError: function (err) {
                console.error('PayPal: Button error', err);
                container.innerHTML = '<div class="alert alert-danger"><i class="bi bi-exclamation-triangle"></i> PayPal encountered an error. Please try again.</div>';
            },

            onCancel: function (data) {
                console.log('PayPal: Payment cancelled by user');
            }
        });

        await currentButtonInstance.render('#paypal-button-container');
        console.log('PayPal: Buttons rendered successfully');
    } catch (renderErr) {
        console.error('PayPal: Button render error', renderErr);
        container.innerHTML = '<div class="alert alert-warning"><i class="bi bi-info-circle"></i> Unable to load payment buttons. Please refresh the page and try again.</div>';
    }
};

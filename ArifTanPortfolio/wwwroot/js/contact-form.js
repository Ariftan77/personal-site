// =======================================================================================
// FIXED CONTACT FORM JAVASCRIPT - PROPERLY RESETS BUTTON STATE
// =======================================================================================

// Enhanced contact form handling with proper error handling
document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('contactForm');
    if (form) {
        form.addEventListener('submit', handleContactFormSubmit);
        
        // Add real-time validation
        initFormValidation(form);
    }
});

async function handleContactFormSubmit(e) {
    e.preventDefault();
    
    const form = e.target;
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    
    // Show loading state
    setButtonLoadingState(submitBtn, true);
    
    try {
        const formData = new FormData(form);
        
        // Add CSRF token if available
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (csrfToken) {
            formData.append('__RequestVerificationToken', csrfToken);
        }
        
        const response = await fetch(form.action || window.location.pathname, {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'X-CSRF-TOKEN': csrfToken || ''
            }
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const result = await response.json();
        
        if (result.success) {
            showAlert('success', result.message || 'Thank you! Your message has been sent successfully. I\'ll get back to you within 24 hours.');
            form.reset();
            clearAllFieldErrors(form);
            
            // Track successful contact form submission
            trackEvent('contact_form_submit', 'engagement', 'contact_form');
        } else {
            showAlert('danger', result.message || 'Sorry, there was an error sending your message. Please try again or contact me directly via email.');
            
            // Show field-specific errors if available
            if (result.errors && Array.isArray(result.errors)) {
                showFieldErrors(form, result.errors);
            }
        }
    } catch (error) {
        console.error('Contact form error:', error);
        showAlert('danger', 'Sorry, there was an error sending your message. Please check your connection and try again, or contact me directly via email.');
    } finally {
        // CRITICAL: Always reset button state regardless of success/failure
        setButtonLoadingState(submitBtn, false, originalText);
    }
}

function setButtonLoadingState(button, isLoading, originalText = null) {
    if (isLoading) {
        button.disabled = true;
        button.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Sending...';
    } else {
        button.disabled = false;
        button.innerHTML = originalText || '<i class="fas fa-paper-plane me-2"></i>Send Message';
    }
}

function showAlert(type, message) {
    // Remove existing alerts first
    const existingAlerts = document.querySelectorAll('.alert');
    existingAlerts.forEach(alert => alert.remove());
    
    const alertContainer = getOrCreateAlertContainer();
    
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.style.marginBottom = '10px';
    alert.innerHTML = `
        <div class="d-flex align-items-center">
            <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-triangle'} me-2"></i>
            <div>${message}</div>
        </div>
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    alertContainer.appendChild(alert);
    
    // Scroll alert into view
    alert.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    
    // Auto-dismiss success messages after 5 seconds
    if (type === 'success') {
        setTimeout(() => {
            if (alert.parentNode) {
                alert.remove();
            }
        }, 5000);
    }
}

function getOrCreateAlertContainer() {
    let container = document.getElementById('alertContainer');
    
    if (!container) {
        container = document.createElement('div');
        container.id = 'alertContainer';
        container.style.position = 'fixed';
        container.style.top = '20px';
        container.style.right = '20px';
        container.style.zIndex = '1050';
        container.style.maxWidth = '400px';
        document.body.appendChild(container);
    }
    
    return container;
}

function initFormValidation(form) {
    const inputs = form.querySelectorAll('input, textarea, select');
    
    inputs.forEach(input => {
        input.addEventListener('blur', validateField);
        input.addEventListener('input', clearFieldError);
    });
    
    // Character counter for message field
    const messageField = form.querySelector('#Contact_Message');
    if (messageField) {
        setupCharacterCounter(messageField);
    }
}

function validateField(e) {
    const field = e.target;
    const value = field.value.trim();
    
    // Clear previous errors
    clearFieldError(e);
    
    let isValid = true;
    let errorMessage = '';
    
    // Required field validation
    if (field.hasAttribute('required') && !value) {
        isValid = false;
        errorMessage = 'This field is required.';
    }
    // Email validation
    else if (field.type === 'email' && value && !isValidEmail(value)) {
        isValid = false;
        errorMessage = 'Please enter a valid email address.';
    }
    // Message length validation
    else if (field.name === 'Contact.Message' && value && value.length < 10) {
        isValid = false;
        errorMessage = 'Message must be at least 10 characters long.';
    }
    // Message too long validation
    else if (field.name === 'Contact.Message' && value.length > 2000) {
        isValid = false;
        errorMessage = 'Message is too long. Please keep it under 2000 characters.';
    }
    
    if (!isValid) {
        showFieldError(field, errorMessage);
    }
    
    return isValid;
}

function showFieldError(field, message) {
    field.classList.add('is-invalid');
    
    let errorElement = field.parentNode.querySelector('.invalid-feedback');
    if (!errorElement) {
        errorElement = document.createElement('div');
        errorElement.className = 'invalid-feedback';
        field.parentNode.appendChild(errorElement);
    }
    
    errorElement.textContent = message;
    errorElement.style.display = 'block';
}

function clearFieldError(e) {
    const field = e.target;
    field.classList.remove('is-invalid');
    
    const errorElement = field.parentNode.querySelector('.invalid-feedback');
    if (errorElement) {
        errorElement.style.display = 'none';
    }
}

function clearAllFieldErrors(form) {
    const fields = form.querySelectorAll('input, textarea, select');
    fields.forEach(field => {
        field.classList.remove('is-invalid');
        const errorElement = field.parentNode.querySelector('.invalid-feedback');
        if (errorElement) {
            errorElement.style.display = 'none';
        }
    });
}

function showFieldErrors(form, errors) {
    errors.forEach(error => {
        if (error.Field) {
            const field = form.querySelector(`[name="${error.Field}"]`);
            if (field && error.Errors && error.Errors.length > 0) {
                showFieldError(field, error.Errors[0]);
            }
        }
    });
}

function setupCharacterCounter(textarea) {
    const maxLength = textarea.getAttribute('maxlength') || 2000;
    
    // Create counter element
    const counterElement = document.createElement('div');
    counterElement.className = 'character-counter text-muted small mt-1';
    textarea.parentNode.appendChild(counterElement);
    
    // Update counter function
    const updateCounter = () => {
        const currentLength = textarea.value.length;
        counterElement.textContent = `${currentLength}/${maxLength} characters`;
        
        // Change color based on length
        if (currentLength > maxLength * 0.9) {
            counterElement.className = 'character-counter text-warning small mt-1';
        } else if (currentLength > maxLength * 0.8) {
            counterElement.className = 'character-counter text-info small mt-1';
        } else {
            counterElement.className = 'character-counter text-muted small mt-1';
        }
    };
    
    // Attach events
    textarea.addEventListener('input', updateCounter);
    textarea.addEventListener('keyup', updateCounter);
    
    // Initial update
    updateCounter();
}

function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function trackEvent(action, category, label) {
    // Google Analytics 4 tracking
    if (typeof gtag !== 'undefined') {
        gtag('event', action, {
            'event_category': category,
            'event_label': label
        });
    }
    
    // Facebook Pixel tracking (if available)
    if (typeof fbq !== 'undefined') {
        fbq('track', 'Contact');
    }
}

// Auto-focus on first field when page loads
document.addEventListener('DOMContentLoaded', function() {
    const firstField = document.querySelector('#Contact_Name');
    if (firstField) {
        setTimeout(() => {
            firstField.focus();
        }, 500);
    }
});

// Copy email to clipboard functionality
function copyEmail(email = 'ariftan7788@gmail.com') {
    if (navigator.clipboard) {
        navigator.clipboard.writeText(email).then(function() {
            showAlert('success', 'Email address copied to clipboard!');
        }).catch(function() {
            fallbackCopyEmail(email);
        });
    } else {
        fallbackCopyEmail(email);
    }
}

function fallbackCopyEmail(email) {
    // Fallback for older browsers
    const textArea = document.createElement('textarea');
    textArea.value = email;
    document.body.appendChild(textArea);
    textArea.select();
    
    try {
        document.execCommand('copy');
        showAlert('success', 'Email address copied to clipboard!');
    } catch (err) {
        showAlert('info', `Email: ${email}`);
    }
    
    document.body.removeChild(textArea);
}
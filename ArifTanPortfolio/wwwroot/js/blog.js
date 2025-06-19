// Blog-specific JavaScript functionality - wwwroot/js/blog.js

document.addEventListener('DOMContentLoaded', function() {
    initBlogFeatures();
});

function initBlogFeatures() {
    initReadingProgress();
    initCodeCopyButtons();
    initSmoothScrolling();
    initTableOfContents();
    initImageZoom();
    initSocialSharing();
    initSearchHighlight();
    initEstimatedReadingTime();
}

// Reading Progress Bar
function initReadingProgress() {
    if (!document.querySelector('.blog-content')) return;

    // Create progress bar
    const progressBar = document.createElement('div');
    progressBar.className = 'reading-progress';
    document.body.appendChild(progressBar);

    // Update progress on scroll
    window.addEventListener('scroll', throttle(updateReadingProgress, 16));

    function updateReadingProgress() {
        const article = document.querySelector('.blog-content');
        if (!article) return;

        const articleTop = article.offsetTop;
        const articleHeight = article.offsetHeight;
        const windowHeight = window.innerHeight;
        const scrollTop = window.pageYOffset;

        const totalReadableHeight = articleHeight - windowHeight;
        const readableScrollTop = scrollTop - articleTop;

        const progress = Math.max(0, Math.min(100, (readableScrollTop / totalReadableHeight) * 100));
        progressBar.style.width = progress + '%';
    }
}

// Code Copy Buttons
function initCodeCopyButtons() {
    const codeBlocks = document.querySelectorAll('.blog-content pre');
    
    codeBlocks.forEach(block => {
        const button = document.createElement('button');
        button.className = 'copy-button';
        button.innerHTML = '<i class="fas fa-copy"></i> Copy';
        button.setAttribute('aria-label', 'Copy code to clipboard');
        
        button.addEventListener('click', async () => {
            const code = block.querySelector('code').textContent;
            
            try {
                await navigator.clipboard.writeText(code);
                button.innerHTML = '<i class="fas fa-check"></i> Copied!';
                button.classList.add('btn-success');
                
                setTimeout(() => {
                    button.innerHTML = '<i class="fas fa-copy"></i> Copy';
                    button.classList.remove('btn-success');
                }, 2000);
            } catch (err) {
                console.error('Failed to copy code:', err);
                fallbackCopyTextToClipboard(code, button);
            }
        });
        
        block.appendChild(button);
    });
}

// Fallback copy method for older browsers
function fallbackCopyTextToClipboard(text, button) {
    const textArea = document.createElement('textarea');
    textArea.value = text;
    textArea.style.position = 'fixed';
    textArea.style.left = '-999999px';
    textArea.style.top = '-999999px';
    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();
    
    try {
        document.execCommand('copy');
        button.innerHTML = '<i class="fas fa-check"></i> Copied!';
        button.classList.add('btn-success');
        
        setTimeout(() => {
            button.innerHTML = '<i class="fas fa-copy"></i> Copy';
            button.classList.remove('btn-success');
        }, 2000);
    } catch (err) {
        console.error('Fallback copy failed:', err);
        button.innerHTML = '<i class="fas fa-times"></i> Failed';
        setTimeout(() => {
            button.innerHTML = '<i class="fas fa-copy"></i> Copy';
        }, 2000);
    }
    
    document.body.removeChild(textArea);
}

// Smooth Scrolling for Anchor Links
function initSmoothScrolling() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Table of Contents Generation
function initTableOfContents() {
    const article = document.querySelector('.blog-content');
    if (!article) return;

    const headings = article.querySelectorAll('h1, h2, h3, h4, h5, h6');
    if (headings.length < 3) return; // Only show TOC if there are enough headings

    const tocContainer = document.createElement('div');
    tocContainer.className = 'table-of-contents card border-0 shadow-sm mb-4';
    tocContainer.innerHTML = `
        <div class="card-header bg-primary text-white">
            <h6 class="mb-0"><i class="fas fa-list me-2"></i>Table of Contents</h6>
        </div>
        <div class="card-body">
            <nav class="toc-nav"></nav>
        </div>
    `;

    const tocNav = tocContainer.querySelector('.toc-nav');
    const tocList = document.createElement('ul');
    tocList.className = 'list-unstyled mb-0';

    headings.forEach((heading, index) => {
        // Generate ID if not present
        if (!heading.id) {
            heading.id = `heading-${index}`;
        }

        const li = document.createElement('li');
        li.className = `toc-item toc-${heading.tagName.toLowerCase()}`;
        
        const link = document.createElement('a');
        link.href = `#${heading.id}`;
        link.textContent = heading.textContent;
        link.className = 'text-decoration-none';
        
        li.appendChild(link);
        tocList.appendChild(li);
    });

    tocNav.appendChild(tocList);

    // Insert TOC after the first paragraph or at the beginning
    const firstParagraph = article.querySelector('p');
    if (firstParagraph) {
        firstParagraph.parentNode.insertBefore(tocContainer, firstParagraph.nextSibling);
    } else {
        article.insertBefore(tocContainer, article.firstChild);
    }

    // Highlight current section
    window.addEventListener('scroll', throttle(highlightCurrentSection, 100));

    function highlightCurrentSection() {
        const scrollPosition = window.scrollY + 100;
        
        tocList.querySelectorAll('a').forEach(link => {
            link.classList.remove('active', 'text-primary', 'fw-bold');
            link.classList.add('text-muted');
        });

        for (let i = headings.length - 1; i >= 0; i--) {
            if (headings[i].offsetTop <= scrollPosition) {
                const activeLink = tocList.querySelector(`a[href="#${headings[i].id}"]`);
                if (activeLink) {
                    activeLink.classList.remove('text-muted');
                    activeLink.classList.add('active', 'text-primary', 'fw-bold');
                }
                break;
            }
        }
    }
}

// Image Zoom on Click
function initImageZoom() {
    const images = document.querySelectorAll('.blog-content img');
    
    images.forEach(img => {
        img.style.cursor = 'pointer';
        img.addEventListener('click', () => {
            openImageModal(img);
        });
    });
}

function openImageModal(img) {
    const modal = document.createElement('div');
    modal.className = 'image-modal';
    modal.innerHTML = `
        <div class="modal-backdrop" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.8); z-index: 9999; display: flex; align-items: center; justify-content: center; cursor: pointer;">
            <img src="${img.src}" alt="${img.alt}" style="max-width: 90%; max-height: 90%; object-fit: contain; border-radius: 8px; box-shadow: 0 10px 30px rgba(0,0,0,0.5);">
            <button class="btn btn-light position-absolute" style="top: 20px; right: 20px; z-index: 10000;" aria-label="Close">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;
    
    document.body.appendChild(modal);
    document.body.style.overflow = 'hidden';
    
    modal.addEventListener('click', () => {
        document.body.removeChild(modal);
        document.body.style.overflow = '';
    });
    
    // Close on Escape key
    const escapeHandler = (e) => {
        if (e.key === 'Escape') {
            modal.click();
            document.removeEventListener('keydown', escapeHandler);
        }
    };
    document.addEventListener('keydown', escapeHandler);
}

// Enhanced Social Sharing
function initSocialSharing() {
    // Add click handlers for existing social share buttons
    document.querySelectorAll('.social-share-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            const url = btn.href;
            openShareWindow(url);
        });
    });
}

function openShareWindow(url) {
    const width = 600;
    const height = 400;
    const left = (window.screen.width / 2) - (width / 2);
    const top = (window.screen.height / 2) - (height / 2);
    
    window.open(url, 'share', `
        width=${width},
        height=${height},
        left=${left},
        top=${top},
        toolbar=0,
        menubar=0,
        location=0,
        status=0,
        scrollbars=1,
        resizable=1
    `);
}

// Search Term Highlighting
function initSearchHighlight() {
    const urlParams = new URLSearchParams(window.location.search);
    const searchTerm = urlParams.get('search') || urlParams.get('q');
    
    if (searchTerm && searchTerm.length > 2) {
        highlightSearchTerms(searchTerm);
    }
}

function highlightSearchTerms(searchTerm) {
    const content = document.querySelector('.blog-content');
    if (!content) return;

    const regex = new RegExp(`(${escapeRegExp(searchTerm)})`, 'gi');
    
    walkTextNodes(content, (textNode) => {
        if (regex.test(textNode.textContent)) {
            const highlightedHTML = textNode.textContent.replace(regex, '<mark class="search-highlight">$1</mark>');
            const wrapper = document.createElement('span');
            wrapper.innerHTML = highlightedHTML;
            textNode.parentNode.replaceChild(wrapper, textNode);
        }
    });
}

function walkTextNodes(node, callback) {
    if (node.nodeType === 3) { // Text node
        callback(node);
    } else {
        for (let i = 0; i < node.childNodes.length; i++) {
            walkTextNodes(node.childNodes[i], callback);
        }
    }
}

function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

// Estimated Reading Time Calculation
function initEstimatedReadingTime() {
    const content = document.querySelector('.blog-content');
    if (!content) return;

    const text = content.textContent || content.innerText;
    const wordCount = text.trim().split(/\s+/).length;
    const readingTimeMinutes = Math.ceil(wordCount / 200); // Average 200 words per minute

    // Update existing reading time display or create one
    const existingDisplay = document.querySelector('.reading-time');
    if (existingDisplay) {
        existingDisplay.textContent = `${readingTimeMinutes} min read`;
    }
}

// Utility Functions
function throttle(func, limit) {
    let inThrottle;
    return function() {
        const args = arguments;
        const context = this;
        if (!inThrottle) {
            func.apply(context, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}

function debounce(func, wait, immediate) {
    let timeout;
    return function() {
        const context = this, args = arguments;
        const later = function() {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        const callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
}

// Blog Search Enhancement
function initBlogSearch() {
    const searchInput = document.querySelector('#blogSearch');
    if (!searchInput) return;

    const searchResults = document.createElement('div');
    searchResults.className = 'search-results position-absolute bg-white border rounded shadow-lg';
    searchResults.style.cssText = `
        top: 100%;
        left: 0;
        right: 0;
        max-height: 300px;
        overflow-y: auto;
        z-index: 1000;
        display: none;
    `;
    
    searchInput.parentNode.appendChild(searchResults);

    let searchTimeout;
    searchInput.addEventListener('input', function(e) {
        clearTimeout(searchTimeout);
        const query = e.target.value.trim();
        
        if (query.length < 2) {
            searchResults.style.display = 'none';
            return;
        }

        searchTimeout = setTimeout(() => {
            performSearch(query, searchResults);
        }, 300);
    });

    // Hide results when clicking outside
    document.addEventListener('click', function(e) {
        if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
            searchResults.style.display = 'none';
        }
    });
}

async function performSearch(query, resultsContainer) {
    try {
        // This would typically make an API call to search
        // For now, we'll use client-side search of visible blog posts
        const blogPosts = document.querySelectorAll('.blog-post-item');
        const results = [];

        blogPosts.forEach(post => {
            const title = post.querySelector('.blog-title')?.textContent || '';
            const content = post.querySelector('.blog-excerpt')?.textContent || '';
            const tags = post.querySelector('.blog-tags')?.textContent || '';

            if (title.toLowerCase().includes(query.toLowerCase()) ||
                content.toLowerCase().includes(query.toLowerCase()) ||
                tags.toLowerCase().includes(query.toLowerCase())) {
                
                const link = post.querySelector('a[href*="/blog/"]')?.href;
                if (link) {
                    results.push({
                        title: title,
                        excerpt: content.substring(0, 100) + '...',
                        url: link
                    });
                }
            }
        });

        displaySearchResults(results, resultsContainer, query);
    } catch (error) {
        console.error('Search error:', error);
    }
}

function displaySearchResults(results, container, query) {
    if (results.length === 0) {
        container.innerHTML = `
            <div class="p-3 text-muted">
                <i class="fas fa-search me-2"></i>No results found for "${query}"
            </div>
        `;
    } else {
        container.innerHTML = results.map(result => `
            <a href="${result.url}" class="d-block p-3 text-decoration-none border-bottom">
                <div class="fw-bold text-dark">${highlightQuery(result.title, query)}</div>
                <div class="small text-muted">${highlightQuery(result.excerpt, query)}</div>
            </a>
        `).join('');
    }
    
    container.style.display = 'block';
}

function highlightQuery(text, query) {
    const regex = new RegExp(`(${escapeRegExp(query)})`, 'gi');
    return text.replace(regex, '<mark>$1</mark>');
}

// Initialize enhanced search if search input exists
document.addEventListener('DOMContentLoaded', function() {
    initBlogSearch();
});

// Add CSS for search highlighting
const searchStyles = document.createElement('style');
searchStyles.textContent = `
    .search-highlight {
        background-color: #fff3cd;
        padding: 2px 4px;
        border-radius: 3px;
    }
    
    .toc-item.toc-h1 { margin-left: 0; }
    .toc-item.toc-h2 { margin-left: 1rem; }
    .toc-item.toc-h3 { margin-left: 2rem; }
    .toc-item.toc-h4 { margin-left: 3rem; }
    .toc-item.toc-h5 { margin-left: 4rem; }
    .toc-item.toc-h6 { margin-left: 5rem; }
    
    .toc-item a {
        display: block;
        padding: 0.25rem 0;
        font-size: 0.9rem;
        transition: all 0.3s ease;
    }
    
    .toc-item a:hover {
        color: #007bff !important;
    }
`;
document.head.appendChild(searchStyles);
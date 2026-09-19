// Editor helpers. Deliberately tiny: the editor's logic lives in C#, and this file only answers
// the three questions the browser alone can answer - where things ended up on screen, and who owns
// the pointer during a drag.
//
// The public pages load none of this. The envelope and the gallery stay script-free.

/** The card's box in viewport pixels, so pointer deltas can be turned into percentages. */
export function cardRect(card) {
    const r = card.getBoundingClientRect();
    return { x: r.left, y: r.top, w: r.width, h: r.height };
}

/**
 * Where every element actually landed, as percentages of the card.
 *
 * The editor needs this because a text box has no height of its own in the model - a poem is as
 * tall as the poem. Only the browser knows how tall that turned out, so the selection frame is
 * drawn from what was measured rather than from what was asked for.
 *
 * Rotated elements report their axis-aligned bounds, which is what getBoundingClientRect gives;
 * the frame around a tilted name is therefore slightly larger than the name itself.
 */
export function measure(card) {
    const r = card.getBoundingClientRect();
    if (r.width === 0 || r.height === 0) return [];

    const out = [];
    for (const node of card.querySelectorAll('[data-el]')) {
        const b = node.getBoundingClientRect();
        out.push({
            id: node.dataset.el,
            x: ((b.left - r.left) / r.width) * 100,
            y: ((b.top - r.top) / r.height) * 100,
            w: (b.width / r.width) * 100,
            h: (b.height / r.height) * 100,
        });
    }
    return out;
}

/**
 * Keeps pointer events coming to the handle even when the pointer leaves it, which is the whole
 * reason a drag does not fall apart when you move faster than the element follows.
 */
export function capture(element, pointerId) {
    try { element.setPointerCapture(pointerId); } catch { /* the pointer is already gone */ }
}

export function release(element, pointerId) {
    try { element.releasePointerCapture(pointerId); } catch { /* nothing to release */ }
}

/** Puts the caret in a box the editor just switched into text-entry mode. */
export function focusAndSelect(element) {
    element.focus();
    const range = document.createRange();
    range.selectNodeContents(element);
    const selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(range);
}

/** Plain text only: the editor stores text, never markup. */
export function readText(element) {
    return element.innerText.replace(/ /g, ' ');
}

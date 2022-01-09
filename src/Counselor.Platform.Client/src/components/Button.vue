<template>
  <button
    @click="click ? click() : () => null"
    :class="disallow ? 'disallowed': ''"
    :type="type || 'button'"
    :disabled="disabled"    
  >
    <slot></slot>
    <slot name="text"></slot>
    <slot name="icon"></slot>
  </button>
</template>

<script>
export default {
    name: 'Button',
    props: {
        text: String,
        click: Function,
        disabled: Boolean,
        disallow: Boolean,
        type: String,
        // tooltip: String
    },
    computed: {
        hoverText() {
            const content = this.tooltip;
            const trigger = 'hover focus';
            const delay = { show: 350, hide: 100 };
            return (content) ? { content, trigger, delay } : undefined;
        },
    }
}
</script>

<style lang="scss" scoped>

</style>
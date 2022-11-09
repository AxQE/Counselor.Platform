<template>
    <div class="node">{{ title }}</div>
</template>

<script>
export default {
    name: 'CommandNodeShort',
    props: {
        title: String
    }
}
</script>

<style lang="scss" scoped>
div {
    background: #494949;
    margin-top: -15px;
    margin-left: -15px;
    margin-right: -15px;
    padding: 10px 15px;
    margin-bottom: 15px;
}
</style>
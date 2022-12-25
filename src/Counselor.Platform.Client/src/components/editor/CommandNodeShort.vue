<template>
    <div class="command-short">{{ title }}</div>
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
.command-short {
    border-radius: 8px;
    border: 2px solid #494949;
    display: block;
    height:60px;
    width: 120px;
    background-color: #c5c343;
    line-height:40px;
    padding: 10px;
    margin: 10px 0px;
    cursor: move;
}
</style>
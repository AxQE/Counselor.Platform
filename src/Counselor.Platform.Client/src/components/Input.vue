<template>
  <div :class="`input-container ${layout}`">
    <label
      v-if="label"
      for="name"
      class="input-label"
    >
      {{label}}
    </label>
    <input
      :type="type"
      :value="value"
      v-on:input="updateValue($event.target.value)"
      :name="name"
      :id="name"
      :placeholder="placeholder"
      class="input-field"
    />
    <p
      v-if="description"
      class="input-description"
    >
    {{ description }}
    </p>
  </div>
</template>

<script>

export default {
  name: 'Input',
  props: {
    value: [String, Number],
    label: String,
    name: String,
    placeholder: String,
    description: String,
    type: {
      default: 'text',
      type: String,
    },
    layout: {
      validator: (value) => ['horizontal', 'vertical'].indexOf(value) !== -1,
      type: String,
      default: 'vertical',
    },
  },
  methods: {
    updateValue(value) {
      this.$emit('input', value);
    },
  },
};
</script>
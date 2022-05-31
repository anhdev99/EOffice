<script>
export default {
  data() {
    return {};
  },
};
</script>

<template>
  <h5 class="fs-16">Popover in a Modal</h5>
  <p class="text-muted">
    You only need to know a little to make a big
    <a
      href="#"
      class="popover-test fw-medium text-decoration-underline link-success"
      data-bs-toggle="popover"
      title="Common Types of Fonts"
      data-bs-content="They're a good choice for more traditional projects."
      data-bs-container="body"
      data-bs-placement="bottom"
      data-bs-original-title="Popover Title"
      >Popover on Click</a
    >
    you do every day. So let's get started. First, some common types of fonts
    and what you need to know about them triggers a popover on click.
  </p>
  <h5 class="fs-16">Tooltips in a Modal</h5>
  <p class="text-muted">
    Opposites attract, and that’s a fact. It’s in our
    <a
      href="#"
      class="tooltip-test text-decoration-underline fw-medium"
      title="Morton Bayer"
      data-bs-container="#exampleModalPopovers"
      data-bs-toggle="tooltip"
      data-bs-original-title="Tooltip title"
      >graphic design</a
    >
    to be interested in the unusual, and that’s why using
    <a
      href="#"
      class="tooltip-test text-decoration-underline"
      title="Web Developers"
      data-bs-toggle="tooltip"
      data-bs-container="#exampleModalPopovers"
      data-bs-original-title="Tooltip title"
      >design</a
    >
    contrasting colors in Graphic Design is a must.
  </p>
</template>

<style></style>

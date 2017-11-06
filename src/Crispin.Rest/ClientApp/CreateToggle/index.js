import React, { Component } from "react";
import { Button, Modal } from "react-bootstrap";

class CreateToggle extends Component {
  constructor() {
    super();
    this.state = { showModal: false };
  }

  render() {
    const open = () => this.setState({ showModal: true });
    const close = () => this.setState({ showModal: false });

    const toggleModal = e => {
      e.preventDefault();
      open();
    };

    return (
      <span>
        <a href="#" onClick={toggleModal}>
          Create Toggle
        </a>
        <Modal show={this.state.showModal} onHide={close}>
          <Modal.Header closeButton>
            <Modal.Title>Create new Toggle</Modal.Title>
          </Modal.Header>
          <Modal.Body>Some form here</Modal.Body>
          <Modal.Footer>
            <Button onClick={close} bsStyle="primary">
              Create
            </Button>
            <Button onClick={close}>Close</Button>
          </Modal.Footer>
        </Modal>
      </span>
    );
  }
}

export default CreateToggle;

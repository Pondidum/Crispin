import { fetch, addTask } from "domain-task";

export const createToggle = (name, description, closeForm) => (
  dispatch,
  getState
) => {
  const options = {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      name: name,
      description: description
    })
  };

  const fetchTask = fetch(`/toggles`, options).then(response => {
    dispatch({ type: "CREATE_TOGGLE_SUCCESS" });
    closeForm();
  });

  addTask(fetchTask);
  dispatch({ type: "CREATE_TOGGLE_REQUEST" });
};
